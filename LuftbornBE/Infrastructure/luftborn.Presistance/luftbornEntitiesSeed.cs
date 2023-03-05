using luftborn.Data.Entities;
using System.Linq;

namespace luftborn.Presistance
{
    public class luftbornEntitiesSeed
    {
        public static void ApiInitialize(luftbornEntities context)
        {
            var initializer = new luftbornEntitiesSeed();
            initializer.SeedEverything(context);
        }

        public void SeedEverything(luftbornEntities context)
        {
            context.Database.EnsureCreated();

            if (context.Employees.Any())
            {
                return; // Db has been seeded
            }
            SeedEmployee(context);
        }
        public void SeedEmployee(luftbornEntities context)
        {
            var employee = new Employee()
            {
                Title = "admin",
                Id = 1,
                Description = "admin",
                Category = "One",
                Tags = "1"
            };
            context.Employees.Add(employee);
            context.SaveChanges();

        }
    }
}
