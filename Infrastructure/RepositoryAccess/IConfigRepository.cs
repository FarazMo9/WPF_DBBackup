using Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RepositoryAccess
{
    public interface IConfigRepository:IBaseRepository<Config,int>
    {
        public Config GetCurrentConfig();
        public void Save(Config config);
    }
}
