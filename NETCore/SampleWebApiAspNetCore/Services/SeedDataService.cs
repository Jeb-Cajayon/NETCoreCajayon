using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(GachaDbContext context)
        {
            context.GachaItems.Add(new GachaEntity() { Number = 1000, Type = "Starter", Name = "Lasagne", Created = DateTime.Now });
            context.GachaItems.Add(new GachaEntity() { Number = 1100, Type = "Main", Name = "Hamburger", Created = DateTime.Now });
            context.GachaItems.Add(new GachaEntity() { Number = 1200, Type = "Dessert", Name = "Spaghetti", Created = DateTime.Now });
            context.GachaItems.Add(new GachaEntity() { Number = 1300, Type = "Starter", Name = "Pizza", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
