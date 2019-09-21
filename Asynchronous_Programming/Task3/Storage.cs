using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3
{
    internal class Storage : IStorage
    {
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var result = new[]
            {
                new Product("Продукт 1", 10),
                new Product("Продукт 2", 11),
                new Product("Продукт 3", 12),
                new Product("Продукт 4", 13),
                new Product("Продукт 5", 14),
            };

            await Task.Delay(1000).ConfigureAwait(false);
            return result;
        }
    }
}
