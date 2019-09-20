using System.Threading.Tasks;

namespace Task4
{
    internal interface IUserRepository
    {
        Task CreateAsync(User user);

        Task<User> GetAsync(int id);

        Task UpdateAsync(User user);

        Task DeleteAsync(int id);
    }
}
