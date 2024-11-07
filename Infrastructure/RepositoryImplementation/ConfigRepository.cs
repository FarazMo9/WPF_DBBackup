using Business.Entities;
using Infrastructure.Cryption;
using Infrastructure.RepositoryAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryImplementation
{
    public class ConfigRepository : BaseRepository<Config, int>, IConfigRepository
    {
        public ConfigRepository(AppDBContext context) : base(context)
        {
        }

        public override Expression<Func<Config, int>> GetKey()
        {
            return item=>item.ID;
        }

        public Config GetCurrentConfig()
        {
            var config = Items().FirstOrDefault();
            if(config is null)
            {
                config = new Config();
            }
            else
            {
                config.FTPUrl = !string.IsNullOrEmpty(config.FTPEncodedUrl) ? CryptionManager.DecryptText(config.FTPEncodedUrl) : string.Empty;
                config.FTPUsername = !string.IsNullOrEmpty(config.FTPEncodedUsername) ? CryptionManager.DecryptText(config.FTPEncodedUsername) : string.Empty;
                config.FTPPassword = !string.IsNullOrEmpty(config.FTPEncodedPassword) ? CryptionManager.DecryptText(config.FTPEncodedPassword) : string.Empty;

            }
            return config;

        }

        public async void Save(Config config)
        {
            if (!Items().Any())
            {
                await Add(config);
                return;
            }

            await Update(config);

        }
    }
}
