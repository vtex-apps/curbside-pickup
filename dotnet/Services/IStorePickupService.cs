using StorePickup.Data;
using StorePickup.Models;
using System.Threading.Tasks;

namespace StorePickup.Services
{
    public interface IStorePickupService
    {
        Task<HookNotification> CreateOrUpdateHook();
        Task<VtexOrder> GetOrderInformation(string orderId);
        Task<bool> ProcessNotification(HookNotification hookNotification);
        Task<bool> SendEmail(StorePickUpConstants.MailTemplateType templateType, VtexOrder order);
        Task<string> GetDefaultTemplate(string templateName);
        Task<bool> AddOrderComment(string message, string orderId);
        Task<string> ProcessLink(string action, string id);
        Task<bool> CreateDefaultTemplate(StorePickUpConstants.MailTemplateType templateType);
    }
}