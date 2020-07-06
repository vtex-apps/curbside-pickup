using StorePickup.Models;
using System.Threading.Tasks;

namespace StorePickup.Services
{
    public interface IStorePickupRepository
    {
        Task<MerchantSettings> GetMerchantSettings();
        Task SetMerchantSettings(MerchantSettings merchantSettings);
        Task<bool> IsInitialized();
        Task SetInitialized();
    }
}