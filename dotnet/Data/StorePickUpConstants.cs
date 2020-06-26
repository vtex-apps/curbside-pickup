using System;
using System.Collections.Generic;
using System.Text;

namespace StorePickup.Data
{
    public class StorePickUpConstants
    {
        public const string AppToken = "X-VTEX-API-AppToken";
        public const string AppKey = "X-VTEX-API-AppKey";
        public const string IsProduction = "X-Vtex-Workspace-Is-Production";
        public const string VtexIdCookie = "VtexIdclientAutCookie";

        public const string EndPointKey = "hook-notification";
        public const string AppName = "store-pickup";

        public const string HookPing = "ping";

        public const string MailService = "http://mailservice.vtex.com.br/api/mail-service/pvt/sendmail";
        public const string Acquirer = "StorePickup";

        public const string GitHubUrl = "http://raw.githubusercontent.com";
        public const string Repository = "vtex-apps/curbside-pickup";
        public const string TemplateFolder = "templates";
        public const string TemplateFileExtension = "hbs";

        public const string CommentDomain = "oms";
        public const string CommentType = "order";

        public const string ViewerCountry = "cloudfront-viewer-country";

        public const string FORWARDED_HEADER = "X-Forwarded-For";
        public const string FORWARDED_HOST = "X-Forwarded-Host";
        public const string APPLICATION_JSON = "application/json";
        public const string HEADER_VTEX_CREDENTIAL = "X-Vtex-Credential";
        public const string AUTHORIZATION_HEADER_NAME = "Authorization";
        public const string PROXY_AUTHORIZATION_HEADER_NAME = "Proxy-Authorization";
        public const string USE_HTTPS_HEADER_NAME = "X-Vtex-Use-Https";
        public const string PROXY_TO_HEADER_NAME = "X-Vtex-Proxy-To";
        public const string VTEX_ACCOUNT_HEADER_NAME = "X-Vtex-Account";
        public const string ENVIRONMENT = "vtexcommercestable";
        public const string LOCAL_ENVIRONMENT = "myvtex";
        public const string VTEX_ID_HEADER_NAME = "VtexIdclientAutCookie";
        public const string HEADER_VTEX_WORKSPACE = "X-Vtex-Workspace";
        public const string APP_SETTINGS = "vtex.StorePickup-pixel";
        public const string ACCEPT = "Accept";
        public const string CONTENT_TYPE = "Content-Type";
        public const string HTTP_FORWARDED_HEADER = "HTTP_X_FORWARDED_FOR";

        public const string DATA_ENTITY = "StorePickup";
        public const string SCHEMA = "StorePickup";

        public class Status
        {
            public const string OrderCreated = "order-created";
            public const string OrderCompleted = "order-completed";
            public const string OnOrderCompleted = "on-order-completed";
            public const string PaymentPending = "payment-pending";
            public const string WaitingForOrderAuthorization = "waiting-for-order-authorization";
            public const string ApprovePayment = "approve-payment";
            public const string PaymentApproved = "payment-approved";
            public const string PaymentDenied = "payment-denied";
            public const string RequestCancel = "request-cancel";
            public const string WaitingForSellerDecision = "waiting-for-seller-decision";
            public const string AuthorizeFullfilment = "authorize-fulfillment";
            public const string OrderCreateError = "order-create-error";
            public const string OrderCreationError = "order-creation-error";
            public const string WindowToCancel = "window-to-cancel";
            public const string ReadyForHandling = "ready-for-handling";
            public const string StartHanding = "start-handling";
            public const string Handling = "handling";
            public const string InvoiceAfterCancellationDeny = "invoice-after-cancellation-deny";
            public const string OrderAccepted = "order-accepted";
            public const string Invoice = "invoice";
            public const string Invoiced = "invoiced";
            public const string Replaced = "replaced";
            public const string CancellationRequested = "cancellation-requested";
            public const string Cancel = "cancel";
            public const string Canceled = "canceled";
        }

        public class MailTemplates
        {
            public const string ReadyForPacking = "ready-for-packing";
            public const string PackageReady = "package-ready";
            public const string AtLocation = "at-location";
        }

        public class OrderCommentText
        {
            public const string ReadyForPacking = "Ready For Packing";
            public const string PackageReady = "Package Ready";
            public const string AtLocation = "At Location";
        }

        public class Domain
        {
            public const string Fulfillment = "Fulfillment";
            public const string Marketplace = "Marketplace";
        }

        public class TemplateType
        {
            public const string Email = "E";
            public const string SMS = "S";
        }

        public enum MailTemplateType
        {
            ReadyForPacking,
            PackageReady,
            AtLocation
        }
    }
}
