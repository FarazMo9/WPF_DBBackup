using Business.Entities;
using Infrastructure.Cryption;
using Infrastructure.RepositoryAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImplementation
{
    public class DatabaseInfoRepository : BaseRepository<DatabaseInfo, int>, IDatabaseInfoRepository
    {
        public DatabaseInfoRepository(AppDBContext context) : base(context)
        {

        }


        public override Expression<Func<DatabaseInfo, int>> GetKey()
        {
            return item => item.ID;
        }

        public async Task<List<DatabaseInfo>> GetDatabases()
        {
            var DBList =await Items().ToListAsync();

            DBList.ForEach(item => item.DecryptedConnectionString = CryptionManager.DecryptText(item.ConnectionString));
            return DBList;
        }

        public async Task<bool> Save(DatabaseInfo model)
        {
            try
            {
                if(!model.IsValid)
                    return false;

                model.ConnectionString = CryptionManager.EncryptText(model.DecryptedConnectionString);

                if (model.ID == 0)
                    await Add(model);
                else
                    await Update(model);
                return true;
            }
            catch
            {
                return false;
            }


        }
    }
}
