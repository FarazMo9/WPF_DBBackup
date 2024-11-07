using Business;
using Business.DTO;
using Business.Entities;
using Infrastructure.RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace Infrastructure.RepositoryImplementation
{
    public class BackupLogRepository : BaseRepository<BackupLog, int>, IBackupLogRepository
    {
        public BackupLogRepository(AppDBContext context) : base(context)
        {

        }

        public override Expression<Func<BackupLog, int>> GetKey()
        {
            return item => item.ID;
        }

        public async void SaveOperationLogs(List<DBBackupResult> logs)
        {
            await AddRange(logs.Select(item => new BackupLog
            {
                DatabaseInfoID = item.Database.ID,
                Date = DateTime.UtcNow,
                IsSuccessful = item.Success,
                Message = item.MessageLog
            }));
        }

    }
}
