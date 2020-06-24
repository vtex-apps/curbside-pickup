namespace StorePickup.Services
{
    public interface ICryptoService
    {
        string DecryptString(string keyText, string cipherText, string saltText);
        string EncryptString(string keyText, string plainText, string saltText);
    }
}