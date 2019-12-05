using System;

namespace CastleProxy.Logic
{
    public interface IService
    {
        Guid CreateRandom();

        BusinessEntity Get(Guid id);
    }
}
