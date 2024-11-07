using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Cryption
{
    public class AESCryption
    {
        //The below encryption and decryption handlers are based on the code reference of the article on :
        //https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

        public static string EncryptText(string key, string plainText)
        {
            byte[] array;
            byte[] iv = new byte[16];


            using (Aes aes = Aes.Create())
            {

                var bytes = Encoding.UTF8.GetBytes(key);
                aes.Key = bytes;
                aes.IV = iv;



                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(array);
        }


        public static string DecryptText(string key, string cipherText)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }


        public static void FileEncrypt(string inputFilePath,string targetFilePath,string key)
        {
            byte[] iv = new byte[16];


            using (Aes aes = Aes.Create())
            {

                var bytes = Encoding.UTF8.GetBytes(key);
                aes.Key = bytes;
                aes.IV = iv;



                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (FileStream fileStream = new FileStream(targetFilePath, FileMode.Create))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream fsIn = new FileStream(inputFilePath, FileMode.Open))
                        {
                            byte[] buffer = new byte[fsIn.Length];
                            int read;
                            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, read);
                            }
                        }
                    

                    }
                }
            }


        }

        public static void FileDecrypt(string inputFilePath, string targetFilePath, string key)
        {
            byte[] iv = new byte[16];

            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (FileStream fileStream = new FileStream(targetFilePath, FileMode.Create))
                {

                    using (CryptoStream cryptoStream = new CryptoStream(fileStream, decryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream fsIn = new FileStream(inputFilePath, FileMode.Open))
                        {
                            byte[] buffer = new byte[fsIn.Length];
                            int read;
                            while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                cryptoStream.Write(buffer, 0, read);
                            }
                        }
                       
                    }
                }
            }

        }
    }
}
