using System;
using System.Threading.Tasks;

namespace Task4
{
    internal class Program
    {
        private const string ConnectionString = @"data source=(localdb)\MSSQLLocalDB;Initial Catalog=Task4Db;Integrated Security=True;";

        private static async Task Main()
        {
            using (var context = new Task4.DbContext(ConnectionString))
            {
                var repository = new UserRepository(context);

                var user1 = new User
                {
                    Name = "User1 Name",
                    Surname = "User 1 surname",
                    Age = 18
                };

                await repository.CreateAsync(user1);
                Console.WriteLine($"Created User: {user1}");

                var dbUser = await repository.GetAsync(user1.Id);
                Console.WriteLine($"User From Db: {dbUser}");

                dbUser.Age = 22;
                await repository.UpdateAsync(dbUser);
                Console.WriteLine($"Updated User: {dbUser}");

                await repository.DeleteAsync(dbUser.Id);
                Console.WriteLine("User Deleted");

                dbUser = await repository.GetAsync(user1.Id);
                Console.WriteLine($"User From Db: {dbUser}");
            }

            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
