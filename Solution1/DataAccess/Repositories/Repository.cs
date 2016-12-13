using System;
using Services.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public abstract class Repository<T>
    {
        protected readonly PlatformManagement context;

        public Repository(PlatformManagement platformManagement)
        {
            context = platformManagement;
        }
    }
}
