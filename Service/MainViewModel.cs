using Business;
using Business.Entities;
using CommunityToolkit.Mvvm.Input;
using FluentFTP;
using Infrastructure;
using Infrastructure.Cryption;
using Infrastructure.RepositoryAccess;
using Infrastructure.RepositoryImplementation;
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
    public class MainViewModel : BaseViewModel
    {
        private readonly IConfigRepository configRepository;
        private readonly IDatabaseInfoRepository databaseInfoRepository;
        private readonly IBackupLogRepository backupLogRepository;
        private FTPManager FTPManager;
        public Action RefreshLogs;
        public Action ReloadConfig;

        public Config CurrentConfig { get; set; }
        public IntervalManager IntervalManager { get; set; }
        public bool IsUploading { get; set; }
        public double Progress { get; set; }
        public bool IsFTPConnecttionAvailable { get; set; }
        public ICommand StartTimerCommand { get; set; }
        public ICommand StopTimerCommand { get; set; }


        public MainViewModel()
        {

            configRepository = Program.GetInstance().ServiceProvider.GetService<IConfigRepository>();
            databaseInfoRepository = Program.GetInstance().ServiceProvider.GetService<IDatabaseInfoRepository>();
            backupLogRepository = Program.GetInstance().ServiceProvider.GetService<IBackupLogRepository>();
            InitializeProperties();
            InitialCommands();

        }

        private void InitialCommands()
        {
            StartTimerCommand = new RelayCommand(() => {
                try
                {
                    IntervalManager.StartTimer();

                    IsUploading = true;
                    NotifyPropertyChanged(nameof(IsUploading));
                    NotifyPropertyChanged(nameof(IntervalManager));
                }
                catch
                {
                    Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);
                }



            });
            StopTimerCommand = new RelayCommand(() => {
                try
                {
                    IntervalManager.StopTimer();
                    IsUploading = false;
                    NotifyPropertyChanged(nameof(IsUploading));
                    NotifyPropertyChanged(nameof(IntervalManager));
                }
                catch
                {
                    Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

                }

            });
            ReloadConfig = () =>
            {
                CurrentConfig = configRepository.GetCurrentConfig();
                InitializeProperties();
                NotifyPropertyChanged(nameof(CurrentConfig));

            };
        }

        private async void InitializeProperties()
        {
            try
            {
                CurrentConfig = configRepository.GetCurrentConfig();
                IsFTPConnecttionAvailable = CurrentConfig.IsFTPAvailable;

                var databases = await databaseInfoRepository.GetDatabases();
                IntervalManager = new IntervalManager(CurrentConfig, databases, (operationLogs) =>
                {
                    backupLogRepository.SaveOperationLogs(operationLogs);
                    RefreshLogs?.Invoke();
                    IsUploading = false;
                    NotifyPropertyChanged(nameof(IsUploading));
                    NotifyPropertyChanged(nameof(FTPManager));

                }, (progressValue) =>
                {
                    Progress = progressValue;
                    NotifyPropertyChanged(nameof(Progress));

                });

            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }

        }


    }
}
