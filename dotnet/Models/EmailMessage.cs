using System;
using System.Collections.Generic;
using System.Text;

namespace StorePickup.Models
{
    public class JsonData
    {
        public VtexOrder order { get; set; }
        public CurbsidePickup curbsidePickup { get; set; }
    }

    public class CurbsidePickup
    {
        public string toEmail { get; set; }
        public string encryptedOrderId { get; set; }
        public string queryArgs { get; set; }
        public string actionLink { get; set; }
        public string cancelLink { get; set; }
    }

    public class EmailMessage
    {
        public object providerName { get; set; }
        public string templateName { get; set; }
        public JsonData jsonData { get; set; }
    }
}
