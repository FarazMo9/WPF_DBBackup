using Business;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Cryption;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
#nullable disable
namespace Application
{
    public class BackupDecryptionViewModel : BaseViewModel
    {
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public Database? DatabaseType { get; set; }
        public string BackupExtension => DatabaseType switch { Database.MySQL => GeneralInfo.MySQLBackupFileExtension, Database.SQLServer => GeneralInfo.SQLServerBackupFileExtension, _ => string.Empty };
        public ICommand SourceBrowseCommand { get; set; }
        public ICommand TargetBrowseCommand { get; set; }
        public ICommand RestoreCommand { get; set; }

        public BackupDecryptionViewModel()
        {
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            SourceBrowseCommand = new RelayCommand(SourceBrowse);
            TargetBrowseCommand = new RelayCommand(TargetBrowse);
            RestoreCommand = new RelayCommand(Restore);
        }

        private void SourceBrowse()
        {
            SourcePath = Program.GetInstance().OpenFileDialogAction?.Invoke();
            NotifyPropertyChanged(nameof(SourcePath));
        }
        private void TargetBrowse()
        {
            TargetPath = Program.GetInstance().OpenFolderDialogAction?.Invoke();
            NotifyPropertyChanged(nameof(TargetPath));
        }

        private void Restore()
        {
            try
            {
                var message = string.Empty;
                if (string.IsNullOrEmpty(SourcePath) || !DatabaseType.HasValue)
                    message = "Please specify the related parameters.";
                else if (Path.GetExtension(SourcePath) != GeneralInfo.EncryptedFileExtension)
                    message = "The encrypted backup file is unknown.";
                else
                {

                    var targetPath = $"{GeneralInfo.RestorePath}\\{Path.GetFileNameWithoutExtension(SourcePath)}{BackupExtension}";

                    var result = CryptionManager.DecryptFile(SourcePath, targetPath);


                    message = result.Message;
                }
                Program.GetInstance().ShowMesssage(message);

            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }


        }
    }
}
