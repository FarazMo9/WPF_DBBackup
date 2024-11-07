using Business;
using Business.Entities;
using Infrastructure.RepositoryAccess;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Service
{
    public class BackupLogViewModel:BaseViewModel
    {
        private IBackupLogRepository backupLogRepository;
        public ObservableCollection<BackupLog> Logs { get; set; } = new ObservableCollection<BackupLog>();

        public BackupLogViewModel()
        {
            backupLogRepository = Program.GetInstance().ServiceProvider.GetService<IBackupLogRepository>();
            LoadBackupLogs();
        }

        public void LoadBackupLogs()
        {
            try
            {
                Logs = new ObservableCollection<BackupLog>(backupLogRepository.Items());
                NotifyPropertyChanged(nameof(Logs));
            }
            catch
            {
                Program.GetInstance().ShowMesssage(GeneralInfo.ErrorMessage);

            }

        }
    }
}
