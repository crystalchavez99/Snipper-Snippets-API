using System.Security.Cryptography;
using System.Text;

namespace Snipper_Snippet_API.Utilities
{
    public class EncryptDecrypt
    {
        public string Encrypt(string plainText)
        {
            byte[] key = Encoding.UTF8.GetBytes("temporary_secret_key");
            byte[] iv = Encoding.UTF8.GetBytes("temporary_init_vector");
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }
                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedText)
        {
            byte[] key = Encoding.UTF8.GetBytes("temporary_secret_key");
            byte[] iv = Encoding.UTF8.GetBytes("temporary_init_vector");
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swEncrypt = new StreamReader(csDecrypt))
                        {
                            return swEncrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
