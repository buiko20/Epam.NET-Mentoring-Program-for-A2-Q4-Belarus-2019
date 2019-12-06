using System;

namespace PostSharpCodeRewriting.Logic
{
    [PostSharpAspect]
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

        public BusinessEntity Get(Guid id)
        {
            var dbEntity = _repository.Get(id);
            return new BusinessEntity { Id = dbEntity.Id, Data = dbEntity.Data };
        }
    }
}
