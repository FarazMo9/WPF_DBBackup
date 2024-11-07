using Business;
using Business.DTO;
using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryAccess
{
    public interface IBackupLogRepository:IBaseRepository<BackupLog,int>
    {
        public void SaveOperationLogs(List<DBBackupResult> logs);
    }
}
