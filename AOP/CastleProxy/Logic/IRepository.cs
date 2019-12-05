namespace CastleProxy.Logic
{
    public interface IRepository
    {
        void Create(DbEntity entity);

        DbEntity Get(long id);
    }
}
