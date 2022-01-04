using DataAccessLayer.Entities;

using Microsoft.EntityFrameworkCore;

using Date = System.DateTime;

namespace DataAccessLayer.Test.TestData
{
    class TeamRepositoryFakeDataInitializer : Interfaces.IDbInitializer
    {
        public int TotalEntitiesCount => 5;

        public void Seed(ModelBuilder modelBuilder)
        {
            Team team1 = new Team { Id = 1, Name = "Name 1", CreatedAt = Date.Now };
            Team team2 = new Team { Id = 2, Name = "Name 2", CreatedAt = Date.Now };
            Team team3 = new Team { Id = 3, Name = "Name 3", CreatedAt = Date.Now };
            Team team4 = new Team { Id = 4, Name = "Name 4", CreatedAt = Date.Now };
            Team team5 = new Team { Id = 5, Name = "Name 5", CreatedAt = Date.Now };

            modelBuilder.Entity<Team>().HasData(team1, team2, team3, team4, team5);
        }
    }
}
