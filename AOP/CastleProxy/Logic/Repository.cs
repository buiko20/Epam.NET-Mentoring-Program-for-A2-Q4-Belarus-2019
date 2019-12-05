using System;
using System.Collections.Generic;
using System.Linq;

namespace CastleProxy.Logic
{
    public class Repository : IRepository
    {
        private readonly IList<DbEntity> _entities= new List<DbEntity>();

        public void Create(DbEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            _entities.Add(entity);
        }

        public DbEntity Get(Guid id)
        {
            return _entities.FirstOrDefault(e => e.Id == id);
        }
    }
}
