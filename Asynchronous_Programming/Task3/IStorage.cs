using System.Collections.Generic;
using System.Threading.Tasks;

namespace Task3
{
    internal interface IStorage
    {
        Task<IEnumerable<Product>> GetProductsAsync();
    }
}
