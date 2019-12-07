using System;
using System.Threading.Tasks;

namespace CastleAsyncProxy.Logic
{
    public interface IService
    {
        Guid CreateRandom();

        Task<BusinessEntity> GetAsync(Guid id);
    }
}
