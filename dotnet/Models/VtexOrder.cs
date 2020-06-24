using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorePickup.Models
{
    public class VtexOrder
    {
        [JsonProperty("emailTracked")]
        public string EmailTracked { get; set; }

        [JsonProperty("approvedBy")]
        public string ApprovedBy { get; set; }

        [JsonProperty("cancelledBy")]
        public string CancelledBy { get; set; }

        [JsonProperty("cancelReason")]
        public string CancelReason { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("marketplaceOrderId")]
        public string MarketplaceOrderId { get; set; }

        [JsonProperty("marketplaceServicesEndpoint")]
        public Uri MarketplaceServicesEndpoint { get; set; }

        [JsonProperty("sellerOrderId")]
        public string SellerOrderId { get; set; }

        [JsonProperty("origin")]
        public string Origin { get; set; }

        [JsonProperty("affiliateId")]
        public string AffiliateId { get; set; }

        [JsonProperty("salesChannel")]
        public string SalesChannel { get; set; }

        [JsonProperty("merchantName")]
        public string MerchantName { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("statusDescription")]
        public string StatusDescription { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("creationDate")]
        public DateTimeOffset CreationDate { get; set; }

        [JsonProperty("lastChange")]
        public DateTimeOffset LastChange { get; set; }

        [JsonProperty("orderGroup")]
        public string OrderGroup { get; set; }

        [JsonProperty("totals")]
        public List<Total> Totals { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("marketplaceItems")]
        public List<string> MarketplaceItems { get; set; }

        [JsonProperty("clientProfileData")]
        public ClientProfileData ClientProfileData { get; set; }

        [JsonProperty("giftRegistryData")]
        public string GiftRegistryData { get; set; }

        [JsonProperty("marketingData")]
        public object MarketingData { get; set; }

        [JsonProperty("ratesAndBenefitsData")]
        public RatesAndBenefitsData RatesAndBenefitsData { get; set; }

        [JsonProperty("shippingData")]
        public ShippingData ShippingData { get; set; }

        [JsonProperty("paymentData")]
        public PaymentData PaymentData { get; set; }

        [JsonProperty("packageAttachment")]
        public PackageAttachment PackageAttachment { get; set; }

        [JsonProperty("sellers")]
        public List<Seller> Sellers { get; set; }

        [JsonProperty("callCenterOperatorData")]
        public string CallCenterOperatorData { get; set; }

        [JsonProperty("followUpEmail")]
        public string FollowUpEmail { get; set; }

        [JsonProperty("lastMessage")]
        public string LastMessage { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("invoiceData")]
        public object InvoiceData { get; set; }

        [JsonProperty("changesAttachment")]
        public ChangesAttachment ChangesAttachment { get; set; }

        [JsonProperty("openTextField")]
        public object OpenTextField { get; set; }

        [JsonProperty("roundingError")]
        public int RoundingError { get; set; }

        [JsonProperty("orderFormId")]
        public string OrderFormId { get; set; }

        [JsonProperty("commercialConditionData")]
        public string CommercialConditionData { get; set; }

        [JsonProperty("isCompleted")]
        public bool IsCompleted { get; set; }

        [JsonProperty("customData")]
        public string CustomData { get; set; }

        [JsonProperty("storePreferencesData")]
        public StorePreferencesData StorePreferencesData { get; set; }

        [JsonProperty("allowCancellation")]
        public bool AllowCancellation { get; set; }

        [JsonProperty("allowEdition")]
        public bool AllowEdition { get; set; }

        [JsonProperty("isCheckedIn")]
        public bool IsCheckedIn { get; set; }

        [JsonProperty("marketplace")]
        public Marketplace Marketplace { get; set; }

        [JsonProperty("authorizedDate")]
        public DateTimeOffset AuthorizedDate { get; set; }

        [JsonProperty("invoicedDate")]
        public string InvoicedDate { get; set; }
    }

    public class ChangesAttachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("changesData")]
        public List<ChangesDatum> ChangesData { get; set; }
    }

    public class ChangesDatum
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("discountValue")]
        public int DiscountValue { get; set; }

        [JsonProperty("incrementValue")]
        public int IncrementValue { get; set; }

        [JsonProperty("itemsAdded")]
        public List<string> ItemsAdded { get; set; }

        [JsonProperty("itemsRemoved")]
        public List<ItemsRemoved> ItemsRemoved { get; set; }

        [JsonProperty("receipt")]
        public Receipt Receipt { get; set; }
    }

    public class ItemsRemoved
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("unitMultiplier")]
        public float UnitMultiplier { get; set; }
    }

    public class Receipt
    {
        [JsonProperty("date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("receipt")]
        public string ReceiptReceipt { get; set; }
    }

    public class ClientProfileData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("documentType")]
        public string DocumentType { get; set; }

        [JsonProperty("document")]
        public string Document { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("corporateName")]
        public string CorporateName { get; set; }

        [JsonProperty("tradeName")]
        public string TradeName { get; set; }

        [JsonProperty("corporateDocument")]
        public string CorporateDocument { get; set; }

        [JsonProperty("stateInscription")]
        public string StateInscription { get; set; }

        [JsonProperty("corporatePhone")]
        public string CorporatePhone { get; set; }

        [JsonProperty("isCorporate")]
        public bool IsCorporate { get; set; }

        [JsonProperty("userProfileId")]
        public Guid UserProfileId { get; set; }

        [JsonProperty("customerClass")]
        public string CustomerClass { get; set; }
    }

    public class Item
    {
        [JsonProperty("uniqueId")]
        public string UniqueId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("ean")]
        public string Ean { get; set; }

        [JsonProperty("lockId")]
        public string LockId { get; set; }

        [JsonProperty("itemAttachment")]
        public ItemAttachment ItemAttachment { get; set; }

        [JsonProperty("attachments")]
        public List<string> Attachments { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("seller")]
        public string Seller { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("refId")]
        public string RefId { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("listPrice")]
        public int ListPrice { get; set; }

        [JsonProperty("manualPrice")]
        public string ManualPrice { get; set; }

        [JsonProperty("priceTags")]
        public List<PriceTag> PriceTags { get; set; }

        [JsonProperty("imageUrl")]
        public Uri ImageUrl { get; set; }

        [JsonProperty("detailUrl")]
        public string DetailUrl { get; set; }

        [JsonProperty("components")]
        public List<string> Components { get; set; }

        [JsonProperty("bundleItems")]
        public List<string> BundleItems { get; set; }

        [JsonProperty("params")]
        public List<string> Params { get; set; }

        [JsonProperty("offerings")]
        public List<string> Offerings { get; set; }

        [JsonProperty("sellerSku")]
        public string SellerSku { get; set; }

        [JsonProperty("priceValidUntil")]
        public string PriceValidUntil { get; set; }

        [JsonProperty("commission")]
        public int Commission { get; set; }

        [JsonProperty("tax")]
        public int Tax { get; set; }

        [JsonProperty("preSaleDate")]
        public string PreSaleDate { get; set; }

        [JsonProperty("additionalInfo")]
        public AdditionalInfo AdditionalInfo { get; set; }

        [JsonProperty("measurementUnit")]
        public string MeasurementUnit { get; set; }

        [JsonProperty("unitMultiplier")]
        public float UnitMultiplier { get; set; }

        [JsonProperty("sellingPrice")]
        public int SellingPrice { get; set; }

        [JsonProperty("isGift")]
        public bool IsGift { get; set; }

        [JsonProperty("shippingPrice")]
        public string ShippingPrice { get; set; }

        [JsonProperty("rewardValue")]
        public int RewardValue { get; set; }

        [JsonProperty("freightCommission")]
        public int FreightCommission { get; set; }

        [JsonProperty("priceDefinitions")]
        public string PriceDefinitions { get; set; }

        [JsonProperty("taxCode")]
        public string TaxCode { get; set; }

        [JsonProperty("parentItemIndex")]
        public string ParentItemIndex { get; set; }

        [JsonProperty("parentAssemblyBinding")]
        public string ParentAssemblyBinding { get; set; }
    }

    public class AdditionalInfo
    {
        [JsonProperty("brandName")]
        public string BrandName { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("categoriesIds")]
        public string CategoriesIds { get; set; }

        [JsonProperty("productClusterId")]
        public string ProductClusterId { get; set; }

        [JsonProperty("commercialConditionId")]
        public string CommercialConditionId { get; set; }

        [JsonProperty("dimension")]
        public Dimension Dimension { get; set; }

        [JsonProperty("offeringInfo")]
        public string OfferingInfo { get; set; }

        [JsonProperty("offeringType")]
        public string OfferingType { get; set; }

        [JsonProperty("offeringTypeId")]
        public string OfferingTypeId { get; set; }
    }

    public class Dimension
    {
        [JsonProperty("cubicweight")]
        public float Cubicweight { get; set; }

        [JsonProperty("height")]
        public float Height { get; set; }

        [JsonProperty("length")]
        public float Length { get; set; }

        [JsonProperty("weight")]
        public float Weight { get; set; }

        [JsonProperty("width")]
        public float Width { get; set; }
    }

    public class ItemAttachment
    {
        [JsonProperty("content")]
        public Con Content { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Con
    {
    }

    public class Marketplace
    {
        [JsonProperty("baseURL")]
        public Uri BaseUrl { get; set; }

        [JsonProperty("isCertified")]
        public string IsCertified { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class PackageAttachment
    {
        [JsonProperty("packages")]
        public List<string> Packages { get; set; }
    }

    public class PaymentData
    {
        [JsonProperty("transactions")]
        public List<Transaction> Transactions { get; set; }
    }

    public class Transaction
    {
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("merchantName")]
        public string MerchantName { get; set; }

        [JsonProperty("payments")]
        public List<Payment> Payments { get; set; }
    }

    public class Payment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("paymentSystem")]
        public string PaymentSystem { get; set; }

        [JsonProperty("paymentSystemName")]
        public string PaymentSystemName { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("installments")]
        public int Installments { get; set; }

        [JsonProperty("referenceValue")]
        public int ReferenceValue { get; set; }

        [JsonProperty("cardHolder")]
        public string CardHolder { get; set; }

        [JsonProperty("cardNumber")]
        public string CardNumber { get; set; }

        [JsonProperty("firstDigits")]
        public string FirstDigits { get; set; }

        [JsonProperty("lastDigits")]
        public string LastDigits { get; set; }

        [JsonProperty("cvv2")]
        public string Cvv2 { get; set; }

        [JsonProperty("expireMonth")]
        public string ExpireMonth { get; set; }

        [JsonProperty("expireYear")]
        public string ExpireYear { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("giftCardId")]
        public string GiftCardId { get; set; }

        [JsonProperty("giftCardName")]
        public string GiftCardName { get; set; }

        [JsonProperty("giftCardCaption")]
        public string GiftCardCaption { get; set; }

        [JsonProperty("redemptionCode")]
        public string RedemptionCode { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("tid")]
        public string Tid { get; set; }

        [JsonProperty("dueDate")]
        public DateTimeOffset? DueDate { get; set; }

        [JsonProperty("connectorResponses")]
        public Con ConnectorResponses { get; set; }
    }

    public class RatesAndBenefitsData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rateAndBenefitsIdentifiers")]
        public List<RateAndBenefitsIdentifier> RateAndBenefitsIdentifiers { get; set; }
    }

    public class RateAndBenefitsIdentifier
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("featured")]
        public bool Featured { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("matchedParameters")]
        public MatchedParameters MatchedParameters { get; set; }

        [JsonProperty("additionalInfo")]
        public object AdditionalInfo { get; set; }
    }

    public class MatchedParameters
    {
        [JsonProperty("category@CatalogSystem", NullValueHandling = NullValueHandling.Ignore)]
        public string CategoryCatalogSystem { get; set; }

        [JsonProperty("slaIds", NullValueHandling = NullValueHandling.Ignore)]
        public string SlaIds { get; set; }
    }

    public class Seller
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }
    }

    public class ShippingData
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("address")]
        public OrderAddress Address { get; set; }

        [JsonProperty("logisticsInfo")]
        public List<LogisticsInfo> LogisticsInfo { get; set; }

        [JsonProperty("trackingHints")]
        public string TrackingHints { get; set; }

        [JsonProperty("selectedAddresses")]
        public List<OrderAddress> SelectedAddresses { get; set; }
    }

    public class OrderAddress
    {
        [JsonProperty("addressType")]
        public string AddressType { get; set; }

        [JsonProperty("receiverName")]
        public string ReceiverName { get; set; }

        [JsonProperty("addressId")]
        public string AddressId { get; set; }

        [JsonProperty("postalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("neighborhood")]
        public string Neighborhood { get; set; }

        [JsonProperty("complement")]
        public string Complement { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("geoCoordinates")]
        public List<string> GeoCoordinates { get; set; }
    }

    public class LogisticsInfo
    {
        [JsonProperty("itemIndex")]
        public int ItemIndex { get; set; }

        [JsonProperty("selectedSla")]
        public string SelectedSla { get; set; }

        [JsonProperty("lockTTL")]
        public string LockTtl { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("listPrice")]
        public int ListPrice { get; set; }

        [JsonProperty("sellingPrice")]
        public int SellingPrice { get; set; }

        [JsonProperty("deliveryWindow")]
        public string DeliveryWindow { get; set; }

        [JsonProperty("deliveryCompany")]
        public string DeliveryCompany { get; set; }

        [JsonProperty("shippingEstimate")]
        public string ShippingEstimate { get; set; }

        [JsonProperty("shippingEstimateDate")]
        public DateTimeOffset ShippingEstimateDate { get; set; }

        [JsonProperty("slas")]
        public List<Sla> Slas { get; set; }

        [JsonProperty("shipsTo")]
        public List<string> ShipsTo { get; set; }

        [JsonProperty("deliveryIds")]
        public List<DeliveryId> DeliveryIds { get; set; }

        [JsonProperty("deliveryChannel")]
        public string DeliveryChannel { get; set; }

        [JsonProperty("pickupStoreInfo")]
        public PickupStoreInfo PickupStoreInfo { get; set; }

        [JsonProperty("addressId")]
        public string AddressId { get; set; }

        [JsonProperty("polygonName")]
        public string PolygonName { get; set; }
    }

    public class DeliveryId
    {
        [JsonProperty("courierId")]
        public string CourierId { get; set; }

        [JsonProperty("courierName")]
        public string CourierName { get; set; }

        [JsonProperty("dockId")]
        public string DockId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("warehouseId")]
        public string WarehouseId { get; set; }
    }

    public class PickupStoreInfo
    {
        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }

        [JsonProperty("address")]
        public OrderAddress Address { get; set; }

        [JsonProperty("dockId")]
        public string DockId { get; set; }

        [JsonProperty("friendlyName")]
        public string FriendlyName { get; set; }

        [JsonProperty("isPickupStore")]
        public bool IsPickupStore { get; set; }
    }

    public class Sla
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shippingEstimate")]
        public string ShippingEstimate { get; set; }

        [JsonProperty("deliveryWindow")]
        public string DeliveryWindow { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("deliveryChannel")]
        public string DeliveryChannel { get; set; }

        [JsonProperty("pickupStoreInfo")]
        public PickupStoreInfo PickupStoreInfo { get; set; }

        [JsonProperty("polygonName")]
        public string PolygonName { get; set; }
    }

    public class StorePreferencesData
    {
        [JsonProperty("countryCode")]
        public string CountryCode { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("currencyFormatInfo")]
        public CurrencyFormatInfo CurrencyFormatInfo { get; set; }

        [JsonProperty("currencyLocale")]
        public int CurrencyLocale { get; set; }

        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }
    }

    public class CurrencyFormatInfo
    {
        [JsonProperty("CurrencyDecimalDigits")]
        public decimal CurrencyDecimalDigits { get; set; }

        [JsonProperty("CurrencyDecimalSeparator")]
        public string CurrencyDecimalSeparator { get; set; }

        [JsonProperty("CurrencyGroupSeparator")]
        public string CurrencyGroupSeparator { get; set; }

        [JsonProperty("CurrencyGroupSize")]
        public int CurrencyGroupSize { get; set; }

        [JsonProperty("StartsWithCurrencySymbol")]
        public bool StartsWithCurrencySymbol { get; set; }
    }

    public class Total
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }
    }

    public class PriceTag
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rate")]
        public decimal Rate { get; set; }

        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("rawValue")]
        public decimal RawValue { get; set; }

        [JsonProperty("jurisCode")]
        public string JurisCode { get; set; }

        [JsonProperty("jurisType")]
        public string JurisType { get; set; }

        [JsonProperty("jurisName")]
        public string JurisName { get; set; }

        [JsonProperty("isPercentual")]
        public bool IsPercentual { get; set; }

        [JsonProperty("identifier")]
        public object Identifier { get; set; }
    }
}
