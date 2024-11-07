using Business;
using CommunityToolkit.Mvvm.Input;
using Infrastructure;
using Infrastructure.Cryption;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
#nullable disable
namespace Service
{
    public class KeySetupViewModel : BaseViewModel
    {
        public UserEncryptionKey EncryptionKey { get; set; } = new UserEncryptionKey() { Password = string.Empty };
        public ICommand SavePasswordCommand { get; set; }
        public ICommand EncryptionKeyInputCommand { get; set; }
        public ICommand EncryptionFileBrowseCommand { get; set; }

        public string EncryptionFilePath { get; set; } = GeneralInfo.NotSpecifiedData;
        public Action CloseDialogWindow;
        public KeySetupViewModel()
        {
            InitialCommands();
        }

        private void InitialCommands()
        {
            SavePasswordCommand = new RelayCommand(SavePassword);
            EncryptionKeyInputCommand = new RelayCommand(EncryptionKeyInput);
            EncryptionFileBrowseCommand=new RelayCommand(EncryptionFileBrowse);
        }

        private void SavePassword()
        {
            try
            {
                if (EncryptionKey.IsValid)
                {
                    CryptionManager.StoreKey(EncryptionKey.Password, EncryptionFilePath);
                    Program.GetInstance().ShowMesssage(GeneralInfo.EncryptionKeyFileCopyCautionMessage);
                    CloseDialogWindow?.Invoke();
                }
            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }


        }

        private void EncryptionKeyInput()
        {
            NotifyPropertyChanged(nameof(EncryptionKey));

        }

        private void EncryptionFileBrowse()
        {
            EncryptionFilePath= Program.GetInstance().OpenFileDialogAction?.Invoke();
            NotifyPropertyChanged(nameof(EncryptionFilePath));

        }

    }
}
