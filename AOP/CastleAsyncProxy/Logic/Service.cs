using System;
using System.Threading.Tasks;

namespace CastleAsyncProxy.Logic
{
    public class Service : IService
    {
        private readonly IRepository _repository;

        public Service(IRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Guid CreateRandom()
        {
            var entity = new DbEntity { Id = Guid.NewGuid(), Data = new Random().Next() };
            _repository.Create(entity);
            return entity.Id;
        }

        public async Task<BusinessEntity> GetAsync(Guid id)
        {
            var dbEntity = await _repository.GetAsync(id).ConfigureAwait(false);
            return new BusinessEntity { Id = dbEntity.Id, Data = dbEntity.Data };
        }
    }
}
