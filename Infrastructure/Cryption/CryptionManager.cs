using Business;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Infrastructure.Cryption
{
    public class CryptionManager
    {

        private const string keyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int randomKeyLength = 16;
        private static string userKeyPath => $"{GeneralInfo.AppDataPath}/userKey{GeneralInfo.EncryptionKeyFileExtension}";
        private static string mainKeyPath => $"{GeneralInfo.AppDataPath}/encryptionKey{GeneralInfo.EncryptionKeyFileExtension}";


        #region Key Manager
        public static OperationResult StoreKey(string userKey, string encryptionFilePath = null)
        {
            try
            {
                var random = new Random();
                var randomKey = new string(Enumerable.Repeat(keyChars, randomKeyLength)
                    .Select(s => s[random.Next(s.Length)]).ToArray());


                if (File.Exists(userKeyPath))
                    File.Delete(userKeyPath);
                if (File.Exists(mainKeyPath))
                    File.Delete(mainKeyPath);

                var base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes(userKey));
                File.WriteAllBytes(userKeyPath, Encoding.UTF8.GetBytes(base64Key));

                if (File.Exists(encryptionFilePath) && Path.GetExtension(encryptionFilePath)== GeneralInfo.EncryptionKeyFileExtension)
                {
                    File.Move(encryptionFilePath, mainKeyPath, true);
                }
                else
                {
                    var mainKey = AESCryption.EncryptText(userKey, randomKey);
                    File.WriteAllBytes(mainKeyPath, Encoding.UTF8.GetBytes(mainKey));
                }

                return OperationResult.Get(success: true);
            }
            catch
            {
                return OperationResult.Error();

            }


        }

        public static OperationResult GetMainKey()
        {
            try
            {
                if (File.Exists(mainKeyPath) && File.Exists(userKeyPath))
                {

                    var userKeyInBase64 = Encoding.UTF8.GetString(File.ReadAllBytes(userKeyPath));
                    var userKey = Encoding.UTF8.GetString(Convert.FromBase64String(userKeyInBase64));



                    return OperationResult.Get(success: true, result: AESCryption.DecryptText(userKey, Encoding.UTF8.GetString(File.ReadAllBytes(mainKeyPath))));
                }

                return OperationResult.Get(false, "The key config files could not be found.");
            }
            catch
            {
                return OperationResult.Error();

            }
        }
        #endregion

        public static string EncryptText(string text)
        {
            try
            {
                var keyOperation = GetMainKey();
                if (!keyOperation.Success)
                    return string.Empty;
                return AESCryption.EncryptText(keyOperation.Result.ToString(), text);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string DecryptText(string text)
        {
            try
            {
                var keyOperation = GetMainKey();
                if (!keyOperation.Success)
                    return string.Empty;
                return AESCryption.DecryptText(keyOperation.Result.ToString(), text);
            }
            catch
            {
                return string.Empty;

            }
        }

        public static OperationResult EncryptFile(string inputFilePath, string targetFilePath)
        {
            try
            {
                var keyOperation = GetMainKey();
                if (!keyOperation.Success)
                    return OperationResult.Get(success: false, message: GeneralInfo.KeyRetrievalFailureMessage);
                AESCryption.FileEncrypt(inputFilePath, targetFilePath, keyOperation.Result.ToString());
                return OperationResult.Get();
            }
            catch
            {
                return OperationResult.Error();

            }

        }
        public static OperationResult DecryptFile(string inputFilePath, string targetFilePath)
        {
            try
            {
                var keyOperation = GetMainKey();
                if (!keyOperation.Success)
                    return OperationResult.Get(success: false, message: GeneralInfo.KeyRetrievalFailureMessage);
                if (!Directory.Exists(GeneralInfo.RestorePath))
                    Directory.CreateDirectory(GeneralInfo.RestorePath);


                AESCryption.FileDecrypt(inputFilePath, targetFilePath, keyOperation.Result.ToString());

                Process.Start(GeneralInfo.WindowsFileExplorerAppName, GeneralInfo.RestorePath);
                return OperationResult.Get(message: "The backup file descrypted successfully.");

            }
            catch
            {
                return OperationResult.Error();

            }

        }
    }
}
