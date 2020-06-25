using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using StorePickup.Data;
using StorePickup.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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
        private readonly string _applicationName;

        public StorePickupService(IVtexEnvironmentVariableProvider environmentVariableProvider, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, IIOServiceContext context, ICryptoService cryptoService)
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

            this._applicationName =
                $"{this._environmentVariableProvider.ApplicationVendor}.{this._environmentVariableProvider.ApplicationName}";
        }

        public async Task<string> SendEmail(StorePickUpConstants.MailTemplateType templateType, VtexOrder order)
        {
            string templateName = string.Empty;
            switch (templateType)
            {
                case StorePickUpConstants.MailTemplateType.AtLocation:
                    templateName = StorePickUpConstants.MailTemplates.AtLocation;
                    break;
                case StorePickUpConstants.MailTemplateType.PackageReady:
                    templateName = StorePickUpConstants.MailTemplates.PackageReady;
                    break;
                case StorePickUpConstants.MailTemplateType.ReadyForPacking:
                    templateName = StorePickUpConstants.MailTemplates.ReadyForPacking;
                    break;
            }

            bool templateExists = await this.TemplateExists(templateName);
            if(!templateExists)
            {

                EmailTemplate emailTemplate = new EmailTemplate
                {
                    Name = templateName,
                    Type = StorePickUpConstants.TemplateType.Email,
                    Template = new Template
                    {
                        Message = await this.GetDefaultTemplateBody(templateName)
                    }
                };

                await this.CreateOrUpdateTemplate(emailTemplate);
            }

            string encryptedOrderId = _cryptoService.EncryptString(order.ClientProfileData.Email, order.OrderId, _context.Vtex.Account);
            string queryText = $"{order.ClientProfileData.Email}|{encryptedOrderId}";
            string queryArgs = _cryptoService.EncryptString(StorePickUpConstants.AppName, queryText, _context.Vtex.Account);

            EmailMessage emailMessage = new EmailMessage
            {
                templateName = templateName,
                providerName = StorePickUpConstants.Acquirer,
                jsonData = new JsonData
                {
                    to = order.ClientProfileData.Email,
                    encryptedOrderId = encryptedOrderId,
                    queryArgs = queryArgs
                }
            };

            string accountName = _httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME].ToString();
            string message = JsonConvert.SerializeObject(emailMessage);

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{StorePickUpConstants.MailService}?an={accountName}")
            };

            request.Content = new StringContent(message, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON);
            request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, _httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL].ToString());
            HttpClient client = _clientFactory.CreateClient();
            HttpResponseMessage responseMessage = await client.SendAsync(request);
            string responseContent = await responseMessage.Content.ReadAsStringAsync();

            return $"[{responseMessage.StatusCode}] {responseContent}";
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
                    Url = new Uri($"https://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.LOCAL_ENVIRONMENT}.com/{StorePickUpConstants.AppName}")
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

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] Response {response.StatusCode} Content = '{responseContent}' [-]");
            createOrUpdateHookResponse = JsonConvert.DeserializeObject<HookNotification>(responseContent);

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
                                List<LogisticsInfo> pickUpItems = vtexOrder.ShippingData.LogisticsInfo.Where(i => i.PickupStoreInfo != null && i.PickupStoreInfo.IsPickupStore).ToList();
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
                                    Console.WriteLine(await this.AddOrderComment("Order Comment Test Two.", hookNotification.OrderId));
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

        public async Task<string> CreateOrUpdateTemplate(EmailTemplate template)
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

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] Response {response.StatusCode} Content = '{responseContent}' [-]");

            return responseContent;
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
                request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] Response {response.StatusCode} Content = '{responseContent}' [-]");

            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetDefaultTemplateBody(string templateName)
        {
            string templateBody = string.Empty;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{StorePickUpConstants.GitHubUrl}/{StorePickUpConstants.Repository}/{StorePickUpConstants.TemplateFolder}/{templateName}.{StorePickUpConstants.TemplateFileExtension}")
            };

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] Response {response.StatusCode} Content = '{responseContent}' [-]");
            if(response.IsSuccessStatusCode)
            {
                templateBody = responseContent;
            }

            return templateBody;
        }

        public async Task<bool> AddOrderComment(string message, string orderId)
        {
            // POST https://sandboxusdev.myvtex.com/api/do/notes/

            Console.WriteLine("------- Headers -------");
            foreach (var header in this._httpContextAccessor.HttpContext.Request.Headers)
            {
                Console.WriteLine($"{header.Key}: {header.Value}");
            }

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

            //Console.WriteLine($"Sending {jsonSerializedMessage} to http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.LOCAL_ENVIRONMENT}.com/api/do/notes");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.{StorePickUpConstants.LOCAL_ENVIRONMENT}.com/api/do/notes"),
                Content = new StringContent(jsonSerializedMessage, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
            };

            //request.Headers.Add(StorePickUpConstants.USE_HTTPS_HEADER_NAME, "true");
            //string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            string authToken = _context.Vtex.AuthToken;
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[-] Response {response.StatusCode} Content = '{responseContent}' [-]");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ProcessLink(string action, string id)
        {
            string argsText = _cryptoService.DecryptString(action, id, _context.Vtex.Account);
            string[] args = argsText.Split('|');
            string email = args[0];
            string orderId = _cryptoService.DecryptString(email, args[1], _context.Vtex.Account);

            Console.WriteLine($"{email} {orderId}");

            return true;
        }
    }
}
