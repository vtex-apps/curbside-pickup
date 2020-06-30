using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePickup.Models
{
    public partial class AccessToken
    {
        [JsonProperty("authStatus")]
        public string AuthStatus { get; set; }

        [JsonProperty("promptMFA")]
        public bool PromptMfa { get; set; }

        [JsonProperty("clientToken")]
        public object ClientToken { get; set; }

        [JsonProperty("authCookie")]
        public AuthCookie AuthCookie { get; set; }

        [JsonProperty("accountAuthCookie")]
        public object AccountAuthCookie { get; set; }

        [JsonProperty("expiresIn")]
        public long ExpiresIn { get; set; }

        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("phoneNumber")]
        public object PhoneNumber { get; set; }

        [JsonProperty("scope")]
        public object Scope { get; set; }
    }

    public partial class AuthCookie
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }
}
