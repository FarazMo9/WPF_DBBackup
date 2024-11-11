using Business.Entities;
using Infrastructure.RepositoryAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Infrastructure.Cryption;
using Infrastructure.RepositoryImplementation;
using Business;
#nullable disable
namespace Application
{
    public class DatabaseInfoViewModel : BaseViewModel
    {

        private readonly IDatabaseInfoRepository databaseInfoRepository;

        #region Properties

        public DatabaseInfo DatabaseInfo { get; set; }
        public ObservableCollection<DatabaseInfo> Databases { get; set; } = new ObservableCollection<DatabaseInfo>();
        public ObservableCollection<Database> DatabaseTypes { get; set; }=new ObservableCollection<Database>(Enum.GetValues<Database>());
        #endregion


        #region Commands
        public ICommand SaveDatabaseInfoCommand { get; set; }
        public ICommand DeleteDatabaseInfoCommand { get; set; }
        #endregion

        public DatabaseInfoViewModel()
        {
            databaseInfoRepository = Program.GetInstance().ServiceProvider.GetService<IDatabaseInfoRepository>();
            LoadDatabaseInfoItems();
            InitialCommands();
        }

        private void InitialCommands()
        {
            SaveDatabaseInfoCommand = new AsyncRelayCommand(SaveDatabaseInfo);
            DeleteDatabaseInfoCommand = new AsyncRelayCommand(DeleteDatabaseInfo);

        }
        private async void LoadDatabaseInfoItems()
        {
            try
            {
                Databases = new ObservableCollection<DatabaseInfo>(await databaseInfoRepository.GetDatabases());

                NotifyPropertyChanged(nameof(Databases));
            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }


        }

        private async Task SaveDatabaseInfo()
        {
            try
            {
                await databaseInfoRepository.Save(DatabaseInfo);
                NotifyPropertyChanged(nameof(DatabaseInfo));
            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }


        }

        private async Task DeleteDatabaseInfo()
        {
            try
            {
                await databaseInfoRepository.Delete(DatabaseInfo);

                NotifyPropertyChanged(nameof(DatabaseInfo));
                LoadDatabaseInfoItems();
            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }


        }


    }
}
