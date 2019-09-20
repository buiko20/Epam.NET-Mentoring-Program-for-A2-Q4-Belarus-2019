using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Task4
{
    internal class UserRepository : IUserRepository
    {
        private readonly Task4.DbContext _context;

        public UserRepository(Task4.DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<User> GetAsync(int id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id)
                .ConfigureAwait(false);

            if (dbUser != null)
            {
                _context.Entry(dbUser).CurrentValues.SetValues(user);
                _context.Entry(dbUser).State = EntityState.Modified;
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var dbUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == id)
                .ConfigureAwait(false);
            _context.Entry(dbUser).State = EntityState.Deleted;
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
