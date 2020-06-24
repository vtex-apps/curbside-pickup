using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace StorePickup.Services
{
    public class CryptoService : ICryptoService
    {
        public string EncryptString(string keyText, string plainText, string saltText)
        {
            byte[] array;
            byte[] salt = Encoding.ASCII.GetBytes(saltText);
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyText, salt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }

        public string DecryptString(string keyText, string cipherText, string saltText)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);
            byte[] salt = Encoding.ASCII.GetBytes(saltText);
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(keyText, salt);

            using (Aes aes = Aes.Create())
            {
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
