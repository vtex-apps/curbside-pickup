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
        Task<string> SendEmail(StorePickUpConstants.MailTemplateType templateType, VtexOrder order);
        Task<string> GetDefaultTemplateBody(string templateName);
        Task<bool> AddOrderComment(string message, string orderId);
    }
}