using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CastleAsyncProxy.Logic
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

        public async Task<DbEntity> GetAsync(Guid id)
        {
            await Task.Delay(1000).ConfigureAwait(false);
            return _entities.FirstOrDefault(e => e.Id == id);
        }
    }
}
