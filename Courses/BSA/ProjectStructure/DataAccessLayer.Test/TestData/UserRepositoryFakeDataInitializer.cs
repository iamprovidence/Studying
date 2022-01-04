using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

using Date = System.DateTime;

namespace DataAccessLayer.Test.TestData
{
    class UserRepositoryFakeDataInitializer : Interfaces.IDbInitializer
    {
        public int TotalEntitiesCount => 5;

        public void Seed(ModelBuilder modelBuilder)
        {
            User user1 = new User { Id = 1, FirstName = "Name 1", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };
            User user2 = new User { Id = 2, FirstName = "Name 2", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };
            User user3 = new User { Id = 3, FirstName = "Name 3", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };
            User user4 = new User { Id = 4, FirstName = "Name 4", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };
            User user5 = new User { Id = 5, FirstName = "Name 5", LastName = "LastName", Email = "email@mail.com", Birthday = Date.Now, RegisteredAt = Date.Now.AddMonths(5) };

            modelBuilder.Entity<User>().HasData(user1, user2, user3, user4, user5);
        }
    }
}
