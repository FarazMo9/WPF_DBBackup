using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business
{
    public record GeneralInfo
    {
        public const string ErrorMessage = "An error occured while processing the information.";
        public const string SaveMessage = "The info is saved successfully.";
        public const string UnknownDBTypeMessage = "The database type is unknown.";
        public const string KeyRetrievalFailureMessage = "Encryption key could not be retrieved.";
        public const string EncryptionKeyFileCopyCautionMessage = "Please keep a copy of encryptionKey.bin file which exists on the AppData folder of the application.";
        public const string NotSpecifiedData = "Not specified";

        public const string EncryptionKeyFileExtension = ".bin";
        public const string SQLServerBackupFileExtension = ".bak";
        public const string MySQLBackupFileExtension = ".sql";
        public const string EncryptedFileExtension = ".aes";
        public const string CompressionFileExtension = ".zip";

        public const string WindowsFileExplorerAppName = "explorer.exe";
        public static string AppDataPath => $"{Environment.CurrentDirectory}\\AppData";
        public static string RestorePath => $"{AppDataPath}\\Restore";

        public static string BackupPath => $"{AppDataPath}\\Backup";
        public static string ZipFilesPath => $"{AppDataPath}\\ZipFiles";


    }
}
