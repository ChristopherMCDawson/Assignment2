using Assignment2.Models;

namespace Assignment2.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SportsDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Fans.Any())
            {
                return;   // DB has been seeded 
            }

            var fans = new[]
{
            new Fan { FirstName = "Carson", LastName = "Alexander", BirthDate = DateTime.Parse("1995-01-09") },
            new Fan { FirstName = "Meredith", LastName = "Alonso", BirthDate = DateTime.Parse("1992-09-05") },
            new Fan { FirstName = "Arturo", LastName = "Anand", BirthDate = DateTime.Parse("1993-10-09") },
            new Fan { FirstName = "Gytis", LastName = "Barzdukas", BirthDate = DateTime.Parse("1992-01-09") }
};

            foreach (var fan in fans)
            {
                context.Fans.Add(fan);
            }

            context.SaveChanges();

            var sportClubs = new[]
            {
            new SportClub { Id = "A1", Title = "Alpha", Fee = 300 },
            new SportClub { Id = "B1", Title = "Beta", Fee = 130 },
            new SportClub { Id = "O1", Title = "Omega", Fee = 390 }
};

            foreach (var sportClub in sportClubs)
            {
                context.SportClub.Add(sportClub);
            }

            context.SaveChanges();

            var subscriptions = new[]
            {
            new Subscription { FanId = 1, SportClubID = "A1" },
            new Subscription { FanId = 1, SportClubID = "B1" },
            new Subscription { FanId = 1, SportClubID = "O1" },
            new Subscription { FanId = 2, SportClubID = "A1" },
            new Subscription { FanId = 2, SportClubID = "B1" },
            new Subscription { FanId = 3, SportClubID = "A1" }
};

            foreach (var subscription in subscriptions)
            {
                context.Subscriptions.Add(subscription);
            }

            context.SaveChanges();


        }
    }
}
