using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StorePickup.Data;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vtex.Api.Context;

namespace StorePickup.Services
{
    public class StorePickupRepository
    {
        private readonly IVtexEnvironmentVariableProvider _environmentVariableProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IIOServiceContext _context;
        private readonly string _applicationName;


        public StorePickupRepository(IVtexEnvironmentVariableProvider environmentVariableProvider, IHttpContextAccessor httpContextAccessor, IHttpClientFactory clientFactory, IIOServiceContext context)
        {
            this._environmentVariableProvider = environmentVariableProvider ??
                                                throw new ArgumentNullException(nameof(environmentVariableProvider));

            this._httpContextAccessor = httpContextAccessor ??
                                        throw new ArgumentNullException(nameof(httpContextAccessor));

            this._clientFactory = clientFactory ??
                               throw new ArgumentNullException(nameof(clientFactory));

            this._context = context ??
                            throw new ArgumentNullException(nameof(context));

            this._applicationName =
                $"{this._environmentVariableProvider.ApplicationVendor}.{this._environmentVariableProvider.ApplicationName}";

            //this.VerifySchema();
        }

        //public async Task<bool> SavePickUpStatus()
        //{
        //    // PATCH https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/documents

        //    var jsonSerializedListItems = JsonConvert.SerializeObject(pickupInfo);
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Patch,
        //        RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{StorePickUpConstants.DATA_ENTITY}/documents"),
        //        Content = new StringContent(jsonSerializedListItems, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
        //    };

        //    string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
        //    if (authToken != null)
        //    {
        //        request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
        //        request.Headers.Add(StorePickUpConstants.VtexIdCookie, authToken);
        //        request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
        //    }

        //    var client = _clientFactory.CreateClient();
        //    var response = await client.SendAsync(request);
        //    string responseContent = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Save:{response.StatusCode} Id:{documentId}");

        //    return response.IsSuccessStatusCode;
        //}

        //public async Task GetPickUpStatus(string orderId)
        //{
        //    // GET https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/documents/{{id}}
        //    // GET https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/search

        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{StorePickUpConstants.DATA_ENTITY}/search?_fields=id,email,ListItemsWrapper&_schema={StorePickUpConstants.SCHEMA}&email={orderId}")
        //    };

        //    string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
        //    if (authToken != null)
        //    {
        //        request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
        //        request.Headers.Add(StorePickUpConstants.VtexIdCookie, authToken);
        //        request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
        //    }

        //    var client = _clientFactory.CreateClient();
        //    var response = await client.SendAsync(request);
        //    string responseContent = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Get:{response.StatusCode} ");
        //    ResponseListWrapper responseListWrapper = new ResponseListWrapper();
        //    try
        //    {
        //        JArray searchResult = JArray.Parse(responseContent);
        //        for (int l = 0; l < searchResult.Count; l++)
        //        {
        //            JToken listWrapper = searchResult[l];
        //            if (l == 0)
        //            {
        //                responseListWrapper = JsonConvert.DeserializeObject<ResponseListWrapper>(listWrapper.ToString());
        //            }
        //            else
        //            {
        //                ResponseListWrapper listToRemove = JsonConvert.DeserializeObject<ResponseListWrapper>(listWrapper.ToString());
        //                bool removed = await this.DeleteWishList(listToRemove.Id);
        //                _context.Vtex.Logger.Info("WishList", null, $"Removed Id [{listToRemove.Id}] {removed}");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        responseListWrapper.message = $"Error:{ex.Message}: Rsp = {responseContent} ";
        //        Console.WriteLine($"Error:{ex.Message}: Rsp = {responseContent} ");
        //        _context.Vtex.Logger.Error("WishList", null, $"GetWishList response '{responseContent}'", ex);
        //    }

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        responseListWrapper.message = $"Get:{response.StatusCode}: Rsp = {responseContent}";
        //        _context.Vtex.Logger.Info("WishList", null, $"GetWishList response [{response.StatusCode}] '{responseContent}'");
        //    }

        //    return responseListWrapper;
        //}

        //public async Task<bool> DeleteWishList(string documentId)
        //{
        //    // DEL https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/documents/{{id}}

        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Patch,
        //        RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{StorePickUpConstants.DATA_ENTITY}/documents/{documentId}")
        //    };

        //    string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
        //    if (authToken != null)
        //    {
        //        request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
        //        request.Headers.Add(StorePickUpConstants.VtexIdCookie, authToken);
        //        request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
        //    }

        //    var client = _clientFactory.CreateClient();
        //    var response = await client.SendAsync(request);
        //    string responseContent = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Delete:{response.StatusCode} Id:{documentId}");

        //    return response.IsSuccessStatusCode;
        //}

        //public async Task VerifySchema()
        //{
        //    //Console.WriteLine("Verifing Schema");
        //    // https://{{accountName}}.vtexcommercestable.com.br/api/dataentities/{{data_entity_name}}/schemas/{{schema_name}}
        //    var request = new HttpRequestMessage
        //    {
        //        Method = HttpMethod.Get,
        //        RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{StorePickUpConstants.DATA_ENTITY}/schemas/{StorePickUpConstants.SCHEMA}")
        //    };

        //    string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
        //    if (authToken != null)
        //    {
        //        request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
        //        request.Headers.Add(StorePickUpConstants.VtexIdCookie, authToken);
        //        request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
        //    }

        //    var client = _clientFactory.CreateClient();
        //    var response = await client.SendAsync(request);
        //    string responseContent = await response.Content.ReadAsStringAsync();

        //    if (response.IsSuccessStatusCode && !responseContent.Equals(StorePickUpConstants.SCHEMA_JSON))
        //    {
        //        Console.WriteLine("--------------- Applying Schema ---------------");
        //        request = new HttpRequestMessage
        //        {
        //            Method = HttpMethod.Put,
        //            RequestUri = new Uri($"http://{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}.vtexcommercestable.com.br/api/dataentities/{StorePickUpConstants.DATA_ENTITY}/schemas/{StorePickUpConstants.SCHEMA}"),
        //            Content = new StringContent(StorePickUpConstants.SCHEMA_JSON, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
        //        };

        //        if (authToken != null)
        //        {
        //            request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
        //            request.Headers.Add(StorePickUpConstants.VtexIdCookie, authToken);
        //            request.Headers.Add(StorePickUpConstants.PROXY_AUTHORIZATION_HEADER_NAME, authToken);
        //        }

        //        response = await client.SendAsync(request);
        //        responseContent = await response.Content.ReadAsStringAsync();
        //        _context.Vtex.Logger.Info("WishList", null, $"VerifySchema Applying Schema response [{response.StatusCode}] '{responseContent}'");
        //    }

        //    Console.WriteLine($"Schema Response: {response.StatusCode}");
        //}
    }
}
