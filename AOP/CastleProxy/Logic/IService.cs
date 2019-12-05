namespace CastleProxy.Logic
{
    public interface IService
    {
        void Create(BusinessEntity entity);

        BusinessEntity Get(long id);
    }
}
