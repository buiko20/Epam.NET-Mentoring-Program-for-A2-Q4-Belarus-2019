using System;
using System.Threading.Tasks;

namespace CastleAsyncProxy.Logic
{
    public interface IRepository
    {
        void Create(DbEntity entity);

        Task<DbEntity> GetAsync(Guid id);
    }
}
