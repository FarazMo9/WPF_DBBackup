using Business;
using Business.Entities;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Cryption;
using Infrastructure.RepositoryAccess;
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
    public class ConfigViewModel : BaseViewModel
    {

        private readonly IConfigRepository configRepository;


        public Config Config { get; set; }
        public ICommand SaveConfigCommand { get; set; }
        public string ProcessMessage { get; set; }
        public Action ReloadConfig;
        public ConfigViewModel()
        {
            configRepository = Program.GetInstance().ServiceProvider.GetService<IConfigRepository>();
            SaveConfigCommand = new RelayCommand(SaveConfig);
            LoadConfig();
        }

        private void SaveConfig()
        {
            try
            {
                if (!Config.IsValid)
                {
                    NotifyPropertyChanged(nameof(Config));
                    return;
                }
                Config.FTPEncodedUrl = !string.IsNullOrEmpty(Config.FTPUrl) ? CryptionManager.EncryptText(Config.FTPUrl) : string.Empty;
                Config.FTPEncodedUsername = !string.IsNullOrEmpty(Config.FTPUsername) ? CryptionManager.EncryptText(Config.FTPUsername) : string.Empty;
                Config.FTPEncodedPassword = !string.IsNullOrEmpty(Config.FTPPassword) ? CryptionManager.EncryptText(Config.FTPPassword) : string.Empty;
                configRepository.Save(Config);
                NotifyPropertyChanged(nameof(Config));

                ReloadConfig?.Invoke();
                Program.GetInstance().ShowMesssage(GeneralInfo.SaveMessage);

            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);
            }


        }
     
        private void LoadConfig()
        {
            try
            {
                Config = configRepository.GetCurrentConfig();
                NotifyPropertyChanged(nameof(Config));

            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }



        }
    }
}
