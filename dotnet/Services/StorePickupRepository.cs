using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StorePickup.Data;
using StorePickup.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Vtex.Api.Context;

namespace StorePickup.Services
{
    public class StorePickupRepository : IStorePickupRepository
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
        }

        public async Task<MerchantSettings> GetMerchantSettings()
        {
            // Load merchant settings
            // 'http://apps.${region}.vtex.io/${account}/${workspace}/apps/${vendor.appName}/settings'
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_WORKSPACE]}/apps/{StorePickUpConstants.APP_SETTINGS}/settings"),
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

            return JsonConvert.DeserializeObject<MerchantSettings>(responseContent);
        }

        public async Task SetMerchantSettings(MerchantSettings merchantSettings)
        {
            if (merchantSettings == null)
            {
                merchantSettings = new MerchantSettings();
            }

            var jsonSerializedMerchantSettings = JsonConvert.SerializeObject(merchantSettings);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://apps.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_WORKSPACE]}/apps/{StorePickUpConstants.APP_SETTINGS}/settings"),
                Content = new StringContent(jsonSerializedMerchantSettings, Encoding.UTF8, StorePickUpConstants.APPLICATION_JSON)
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
            }

            request.Headers.Add(StorePickUpConstants.AppKey, merchantSettings.AppKey);
            request.Headers.Add(StorePickUpConstants.AppToken, merchantSettings.AppToken);

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);
            Console.WriteLine($"SetMerchantSettings Init?{merchantSettings.Initialized} [{response.StatusCode}]");

            response.EnsureSuccessStatusCode();
        }

        public async Task<bool> IsInitialized()
        {
            bool isInitialized = false;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_WORKSPACE]}/buckets/{this._applicationName}/{StorePickUpConstants.AppName}/files/initialized"),
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
            if (response.IsSuccessStatusCode)
            {
                if (responseContent.Equals("true"))
                {
                    isInitialized = true;
                }
            }

            return isInitialized;
        }

        public async Task SetInitialized()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"http://vbase.{this._environmentVariableProvider.Region}.vtex.io/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.VTEX_ACCOUNT_HEADER_NAME]}/{this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_WORKSPACE]}/buckets/{this._applicationName}/{StorePickUpConstants.AppName}/files/initialized"),
                Content = new StringContent("true", Encoding.UTF8, "text/plain")
            };

            string authToken = this._httpContextAccessor.HttpContext.Request.Headers[StorePickUpConstants.HEADER_VTEX_CREDENTIAL];
            if (authToken != null)
            {
                request.Headers.Add(StorePickUpConstants.AUTHORIZATION_HEADER_NAME, authToken);
                request.Headers.Add(StorePickUpConstants.VTEX_ID_HEADER_NAME, authToken);
            }

            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            response.EnsureSuccessStatusCode();
        }
    }
}
