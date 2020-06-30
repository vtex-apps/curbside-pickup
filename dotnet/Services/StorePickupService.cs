using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using StorePickup.Data;
using StorePickup.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Vtex.Api.Context;

namespace StorePickup.Services
{
    public class StorePickupService : IStorePickupService
    {
        private readonly IVtexEnvironmentVariableProvider _environmentVariableProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IIOServiceContext _context;
        private readonly ICryptoService _cryptoService;
        private readonly IStorePickupRepository _storePickupRepository;
        private readonly string _applicationName;

        public StorePickupService(IVtexEnvironmentVariableProvider environmentVariableProvider, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, IIOServiceContext context, ICryptoService cryptoService, IStorePickupRepository storePickupRepository)
        {
            this._environmentVariableProvider = environmentVariableProvider ??
                                                throw new ArgumentNullException(nameof(environmentVariableProvider));

            this._httpContextAccessor = httpContextAccessor ??
                                        throw new ArgumentNullException(nameof(httpContextAccessor));

            this._clientFactory = clientFactory ??
                               throw new ArgumentNullException(nameof(clientFactory));

            this._context = context ??
                            throw new ArgumentNullException(nameof(context));

            this._cryptoService = cryptoService ??
                            throw new ArgumentNullException(nameof(cryptoService));

            this._storePickupRepository = storePickupRepository ??
                            throw new ArgumentNullException(nameof(storePickupRepository));

            this._applicationName =
                $"{this._environmentVariableProvider.ApplicationVendor}.{this._environmentVariableProvider.ApplicationName}";


        }

        public async Task<string> SendEmail(StorePickUpConstants.MailTemplateType templateType, VtexOrder order)
        {
            MerchantSettings merchantSettings = await _storePickupRepository.GetMerchantSettings();
            if(string.IsNullOrEmpty(merchantSettings.AppKey) || string.IsNullOrEmpty(merchantSettings.AppToken))
            {
                Console.WriteLine("App Settings missing.");
            }

            string responseText = string.Empty;
            string templateName = string.Empty;
            string subjectText = string.Empty;
            string toEmail = string.Empty;
            string shopperEmail = order.ClientProfileData.Email;
            List<LogisticsInfo> pickUpItems = order.ShippingData.LogisticsInfo.Where(i => i.Slas.Any(s => s.PickupStoreInfo.IsPickupStore)).ToList();
            string storeEmail = string.Empty;
            // TODO: Divide notifications by store location.  Send separate email to each.
            foreach (LogisticsInfo logisticsInfo in pickUpItems)
            {
                string selectedSla = logisticsInfo.SelectedSla;
                Sla sla = logisticsInfo.Slas.Where(s => s.Id.Equals(selectedSla)).FirstOrDefault();
                storeEmail = sla.PickupStoreInfo.Address.Complement;
                if(!string.IsNullOrEmpty(storeEmail))
                {
                    break;
                }
            }

            Console.WriteLine($"Store Email = {storeEmail}");
            switch (templateType)
            {
                case StorePickUpConstants.MailTemplateType.AtLocation:
                    templateName = StorePickUpConstants.MailTemplates.AtLocation;
                    subjectText = StorePickUpConstants.TemplateSubject.AtLocation;
                    toEmail = storeEmail;
                    break;
                case StorePickUpConstants.MailTemplateType.PackageReady:
                    templateName = StorePickUpConstants.MailTemplates.PackageReady;
                    subjectText = StorePickUpConstants.TemplateSubject.PackageReady;
                    toEmail = shopperEmail;
                    break;
                case StorePickUpConstants.MailTemplateType.ReadyForPacking:
                    templateName = StorePickUpConstants.MailTemplates.ReadyForPacking;
                    subjectText = StorePickUpConstants.TemplateSubject.ReadyForPacking;
                    toEmail = storeEmail;
                    break;
                case StorePickUpConstants.MailTemplateType.PickedUp:
                    templateName = StorePickUpConstants.MailTemplates.PickedUp;
                    subjectText = StorePickUpConstants.TemplateSubject.PickedUp;
                    toEmail = shopperEmail;
                    break;
            }

            //templateName = "test1";

            string encryptedOrderId = _cryptoService.EncryptString(order.ClientProfileData.Email, order.OrderId, _context.Vtex.Account);
            string queryText = $"{order.ClientProfileData.Email}|{encryptedOrderId}";
            string queryArgs = _cryptoService.EncryptString(templateName, queryText, _context.Vtex.Account);

            EmailMessage emailMessage = new EmailMessage
            {
                templateName = templateName,
                providerName = StorePickUpConstants.Acquirer,
                jsonData = new JsonData
                {
                    to = toEmail,
                    encryptedOrderId = encryptedOrderId,
                    queryArgs = queryArgs
                }
            };

            string accountName = _httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME].ToString();
            string message = JsonConvert.SerializeObject(emailMessage);

            Console.WriteLine($"Email message = {message}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{StorePickUpConstants.MailService}?an={accountName}"),
                Content = new StringContent(message, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            request.Headers.Add(StorePickUpConstants.AppKey, merchantSettings.AppKey);
            request.Headers.Add(StorePickUpConstants.AppToken, merchantSettings.AppToken);

            HttpClient client = _clientFactory.CreateClient();
            try
            {
                HttpResponseMessage responseMessage = await client.SendAsync(request);
                string responseContent = await responseMessage.Content.ReadAsStringAsync();
                responseText = $"[-] SendEmail [{responseMessage.StatusCode}] {responseContent}";
                if(responseMessage.StatusCode.Equals(HttpStatusCode.NotFound))
                {
                    Console.WriteLine($"Template {templateName} not found.  Creating...");
                    string templateBody = await this.GetDefaultTemplateBody(templateName);
                    if (string.IsNullOrWhiteSpace(templateBody))
                    {
                        Console.WriteLine($"Failed to Load Template {templateName}");
                    }
                    else
                    {
                        EmailTemplate emailTemplate = new EmailTemplate
                        {
                            Name = templateName,
                            FriendlyName = subjectText,
                            Type = string.Empty,
                            IsDefaultTemplate = false,
                            IsPersisted = true,
                            IsRemoved = false,
                            Templates = new Templates
                            {
                                Email = new Email
                                {
                                    IsActive = true,
                                    WithError = false,
                                    Message = templateBody,
                                    Subject = subjectText,
                                    To = StorePickUpConstants.EmailTo,
                                    Cc = string.Empty,
                                    Bcc = string.Empty,
                                    Type = StorePickUpConstants.TemplateType.Email,
                                    ProviderId = StorePickUpConstants.ProviderId
                                },
                                Sms = new Sms
                                {
                                    Type = StorePickUpConstants.TemplateType.SMS,
                                    Parameters = new List<object>()
                                }
                            }
                        };

                        // bool templateExists = await this.CreateOrUpdateTemplate(emailTemplate);
                        //if(templateExists)
                        //{
                        //    responseMessage = await client.SendAsync(request);
                        //    responseContent = await responseMessage.Content.ReadAsStringAsync();
                        //    responseText = $"[-] SendEmail Retry [{responseMessage.StatusCode}] {responseContent}";
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                responseText = $"[-] SendEmail Failure [{ex.Message}]";
            }

            Console.WriteLine(responseText);
            return responseText;
        }

        public async Task<HookNotification> CreateOrUpdateHook()
        {
            // POST https://{accountName}.{environment}.com.br/api/orders/hook/config

            HookNotification createOrUpdateHookResponse = new HookNotification();
            OrderHook orderHook = new OrderHook
            {
                Filter = new Filter
                {
                    Status = new List<string>
                    {
                        StorePickUpConstants.Status.ReadyForHandling,
                    }
                },
                Hook = new Hook
                {
                    Headers = new Headers
                    {
                        Key = StorePickUpConstants.EndPointKey
                    },
                    Url = new Uri($"https://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.LOCAL_ENVIRONMENT}.com/{StorePickUpConstants.AppName}/{StorePickUpConstants.EndPointKey}")
                }
            };

            var jsonSerializedOrderHook = JsonConvert.SerializeObject(orderHook);
            //Console.WriteLine($"Hook = {jsonSerializedOrderHook}");
            //Console.WriteLine($"Url = http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/orders/hook/config");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.ENVIRONMENT}.com.br/api/orders/hook/config"),
                Content = new StringContent(jsonSerializedOrderHook, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            MerchantSettings merchantSettings = await _storePickupRepository.GetMerchantSettings();
            request.Headers.Add(StorePickUpConstants.AppKey, merchantSettings.AppKey);
            request.Headers.Add(StorePickUpConstants.AppToken, merchantSettings.AppToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] CreateOrUpdateHook Response {response.StatusCode} Content = '{responseContent}' [-]");
            if (response.IsSuccessStatusCode)
            {
                createOrUpdateHookResponse = JsonConvert.DeserializeObject<HookNotification>(responseContent);
            }

            return createOrUpdateHookResponse;
        }

        public async Task<VtexOrder> GetOrderInformation(string orderId)
        {
            //Console.WriteLine("------- Headers -------");
            //foreach (var header in this._httpContextAccessor.HttpContext.Request.Headers)
            //{
            //    Console.WriteLine($"{header.Key}: {header.Value}");
            //}
            //Console.WriteLine($"http://{this._httpContextAccessor.HttpContext.Request.Headers[Constants.VTEX_ACCOUNT_HEADER_NAME]}.{Constants.ENVIRONMENT}.com.br/api/checkout/pvt/orders/{orderId}");

            VtexOrder vtexOrder = null;

            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.ENVIRONMENT}.com.br/api/checkout/pvt/orders/{orderId}")
                };

                //request.Headers.Add(Constants.ACCEPT, Constants.APPLICATION_JSON);
                //request.Headers.Add(Constants.CONTENT_TYPE, Constants.APPLICATION_JSON);
                string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
                //Console.WriteLine($"Token = '{authToken}'");
                if (authToken != null)
                {
                    request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                    request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
                    request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                }

                //StringBuilder sb = new StringBuilder();

                var client = _clientFactory.CreateClient();
                var response = await client.SendAsync(request);
                string responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    vtexOrder = JsonConvert.DeserializeObject<VtexOrder>(responseContent);
                    Console.WriteLine($"GetOrderInformation: [{response.StatusCode}] ");
                }
                else
                {
                    Console.WriteLine($"GetOrderInformation: [{response.StatusCode}] '{responseContent}'");
                    _context.Vtex.Logger.Info("GetOrderInformation", null, $"Order# {orderId} [{response.StatusCode}] '{responseContent}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetOrderInformation Error: {ex.Message}");
                _context.Vtex.Logger.Error("GetOrderInformation", null, $"Order# {orderId} Error", ex);
            }

            return vtexOrder;
        }

        public async Task<bool> ProcessNotification(HookNotification hookNotification)
        {
            bool success = false;

            switch (hookNotification.Domain)
            {
                case StorePickUpConstants.Domain.Fulfillment:
                    switch (hookNotification.State)
                    {
                        case StorePickUpConstants.Status.ReadyForHandling:
                            VtexOrder vtexOrder = await this.GetOrderInformation(hookNotification.OrderId);
                            if (vtexOrder != null && vtexOrder.ShippingData != null && vtexOrder.ShippingData.LogisticsInfo != null)
                            {
                                List<LogisticsInfo> pickUpItems = vtexOrder.ShippingData.LogisticsInfo.Where(i => i.Slas.Any(s => s.PickupStoreInfo.IsPickupStore)).ToList();
                                if (pickUpItems != null && pickUpItems.Count > 0)
                                {
                                    Console.WriteLine($"{pickUpItems.Count} Items for pickup.");
                                    await this.SendEmail(StorePickUpConstants.MailTemplateType.ReadyForPacking, vtexOrder);
                                    await this.AddOrderComment(StorePickUpConstants.OrderCommentText.ReadyForPacking, vtexOrder.OrderId);
                                }
                                else
                                {
                                    Console.WriteLine("No items for pickup.");
                                    success = true;
                                    //Console.WriteLine($"Template 'test1' exists? {this.TemplateExists("test1").Result}");
                                    //Console.WriteLine($"Get 'test1' {this.GetDefaultTemplateBody("test1").Result}");
                                    //Console.WriteLine(await this.AddOrderComment("Order Comment Test Two.", hookNotification.OrderId));
                                }
                            }
                            break;
                        default:
                            Console.WriteLine($"State {hookNotification.State} not implemeted.");
                            _context.Vtex.Logger.Info("ProcessNotification", null, $"State {hookNotification.State} not implemeted.");
                            break;
                    }
                    break;
                case StorePickUpConstants.Domain.Marketplace:
                    break;
                default:
                    Console.WriteLine($"Domain {hookNotification.Domain} not implemeted.");
                    _context.Vtex.Logger.Info("ProcessNotification", null, $"Domain {hookNotification.Domain} not implemeted.");
                    break;
            }

            return success;
        }

        public async Task<bool> CreateOrUpdateTemplate(EmailTemplate template)
        {
            // POST: "http://hostname/api/template-render/pvt/templates"

            var jsonSerializedTemplate = JsonConvert.SerializeObject(template);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.ENVIRONMENT}.com.br/api/template-render/pvt/templates"),
                Content = new StringContent(jsonSerializedTemplate, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
            }

            MerchantSettings merchantSettings = await _storePickupRepository.GetMerchantSettings();
            request.Headers.Add(StorePickUpConstants.AppKey, merchantSettings.AppKey);
            request.Headers.Add(StorePickUpConstants.AppToken, merchantSettings.AppToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] CreateOrUpdateTemplate Response {response.StatusCode} Content = '{responseContent}' [-]");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> TemplateExists(string templateName)
        {
            // POST: "http://hostname/api/template-render/pvt/templates"

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.myvtex.com/api/template-render/pvt/templates/{templateName}")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                //request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] TemplateExists Response {response.StatusCode} Content = '{responseContent}' [-]");

            return (int)response.StatusCode == StatusCodes.Status200OK;
        }

        public async Task<string> GetDefaultTemplateBody(string templateName)
        {
            string templateBody = string.Empty;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{StorePickUpConstants.GitHubUrl}/{StorePickUpConstants.Repository}/{StorePickUpConstants.TemplateFolder}/{templateName}.{StorePickUpConstants.TemplateFileExtension}")
            };

            string authToken = _context.Vtex.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                //request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                //request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] GetDefaultTemplateBody [{response.StatusCode}] '{responseContent}' [-]");
            if(response.IsSuccessStatusCode)
            {
                templateBody = responseContent;
            }

            return templateBody;
        }

        public async Task<bool> AddOrderComment(string message, string orderId)
        {
            // POST https://sandboxusdev.myvtex.com/api/do/notes/

            //Console.WriteLine("------- Headers -------");
            //foreach (var header in this._httpContextAccessor.HttpContext.Request.Headers)
            //{
            //    Console.WriteLine($"{header.Key}: {header.Value}");
            //}

            OrderComment orderComment = new OrderComment
            {
                Description = message,
                Domain = StorePickUpConstants.CommentDomain,
                Target = new Target
                {
                    Id = orderId,
                    Type = StorePickUpConstants.CommentType,
                    Url = $"/orders/{orderId}"
                }
            };

            var jsonSerializedMessage = JsonConvert.SerializeObject(orderComment);

            Console.WriteLine($"Sending {jsonSerializedMessage} to http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.LOCAL_ENVIRONMENT}.com/api/do/notes");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.LOCAL_ENVIRONMENT}.com/api/do/notes"),
                Content = new StringContent(jsonSerializedMessage, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
            };

            string accessToken = await this.GetAccessToken();

            //request.Headers.Add(StorePickUpConstants.USE_HTTPS_HEADER_NAME, "true");
            //string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            string authToken = _context.Vtex.AuthToken;
            //string authToken = _context.Vtex.AdminUserAuthToken;
            if (authToken != null && accessToken != null)
            {
                //Console.WriteLine($"authToken = [-]{authToken}[-]");
                //Console.WriteLine($"accessToken = [-]{accessToken}[-]");
                //request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, accessToken);
            }
            else
            {
                Console.WriteLine("Missing Token!");
            }

            //MerchantSettings merchantSettings = await _storePickupRepository.GetMerchantSettings();
            ////Console.WriteLine($"Key:[{merchantSettings.AppKey}] | Token:[{merchantSettings.AppToken}]");
            //string appKey = merchantSettings.AppKey;
            //string appToken = merchantSettings.AppToken;
            //request.Headers.Add(StorePickUpConstants.AppKey, appKey);
            //request.Headers.Add(StorePickUpConstants.AppToken, appToken);

            //request.Headers.Add(StorePickUpConstants.CONTENT_TYPE, StorePickUpConstants.APPLICATION_JSON);

            //Console.WriteLine("------- Headers -------");
            //foreach (var header in request.Headers)
            //{
            //    Console.WriteLine($"{header.Key}: {header.Value}");
            //}

            //Console.WriteLine($"Headers: {request.Headers} ");

            var client = _clientFactory.CreateClient();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StorePickUpConstants.APPLICATION_JSON));
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"[-] AddOrderComment Response {response.StatusCode} Content = '{responseContent}' [-]");
            Console.WriteLine($"[-] AddOrderComment Response {response.StatusCode} [-]");

            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetAccessToken()
        {
            string accessToken = string.Empty;
            MerchantSettings merchantSettings = await _storePickupRepository.GetMerchantSettings();

            //curl--location--request GET 'https://vtexid.vtex.com.br/api/vtexid/pub/authenticate/default?user={{your-app-key}}&scope=&pass={{your-app-token}}'

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://vtexid.vtex.com.br/api/vtexid/pub/authenticate/default?user={merchantSettings.AppKey}&scope=&pass={merchantSettings.AppToken}")
            };

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                AccessToken accessTokenObj = JsonConvert.DeserializeObject<AccessToken>(responseContent);
                accessToken = accessTokenObj.AuthCookie.Value;
            }

            //Console.WriteLine(responseContent);

            return accessToken;
        }

        public async Task<string> ProcessLink(string action, string id)
        {
            //Console.WriteLine("------- Headers -------");
            //foreach (var header in this._httpContextAccessor.HttpContext.Request.Headers)
            //{
            //    Console.WriteLine($"{header.Key}: {header.Value}");
            //}

            string baseUrl = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.FORWARDED_HOST];
            string returnUrl = $"https://{baseUrl}/{StorePickUpConstants.AppName}/{StorePickUpConstants.RedirectPage.ThankYou}";
            try
            {
                string argsText = _cryptoService.DecryptString(action, id, _context.Vtex.Account);
                //string argsText = _cryptoService.DecryptString("test1", id, _context.Vtex.Account);
                string[] args = argsText.Split('|');
                string email = args[0];
                string orderId = _cryptoService.DecryptString(email, args[1], _context.Vtex.Account);

                Console.WriteLine($"{email} {orderId}");
                VtexOrder order = await this.GetOrderInformation(orderId);
                bool mailSent = false;
                switch (action)
                {
                    case StorePickUpConstants.MailTemplates.ReadyForPacking:
                        await this.AddOrderComment(StorePickUpConstants.OrderCommentText.ReadyForPacking, orderId);
                        await this.SendEmail(StorePickUpConstants.MailTemplateType.ReadyForPacking, order);
                        break;
                    case StorePickUpConstants.MailTemplates.PackageReady:
                        await this.AddOrderComment(StorePickUpConstants.OrderCommentText.PackageReady, orderId);
                        await this.SendEmail(StorePickUpConstants.MailTemplateType.PackageReady, order);
                        break;
                    case StorePickUpConstants.MailTemplates.AtLocation:
                        await this.AddOrderComment(StorePickUpConstants.OrderCommentText.AtLocation, orderId);
                        await this.SendEmail(StorePickUpConstants.MailTemplateType.AtLocation, order);
                        break;
                    case StorePickUpConstants.MailTemplates.PickedUp:
                        await this.AddOrderComment(StorePickUpConstants.OrderCommentText.PickedUp, orderId);
                        await this.SendEmail(StorePickUpConstants.MailTemplateType.PickedUp, order);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ProcessLink Error {ex.Message}");
                returnUrl = $"https://{baseUrl}/{StorePickUpConstants.AppName}/{StorePickUpConstants.RedirectPage.Error}";
            }

            Console.WriteLine($"returnUrl [{returnUrl}]");
            return returnUrl;
        }
    }
}
