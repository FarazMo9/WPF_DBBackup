using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public record DBBackupResult
    {

        public bool Success { get; set; }
        public DatabaseInfo Database { get; set; }
        public string? BackupFileName { get; set; }
        public string BackupDirectory { get; set; }
        public string BackupFileExtension { get; set; }

        public string MessageLog { get; set; }
        public string SourceBackupPath => $"{BackupDirectory}\\{BackupFileName}{BackupFileExtension}";
        public string EncryptedBackupPath => $"{BackupDirectory}\\{BackupFileName}{GeneralInfo.EncryptedFileExtension}";
        public string EncryptedBackupFileName => $"{BackupFileName}{GeneralInfo.EncryptedFileExtension}";


    }
}
