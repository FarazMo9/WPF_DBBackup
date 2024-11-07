using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryAccess
{
    public interface IDatabaseInfoRepository : IBaseRepository<DatabaseInfo, int>
    {
        public Task<bool> Save(DatabaseInfo model);
        public Task<List<DatabaseInfo>> GetDatabases();
    }
}
