using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public interface ISeedDataService
    {
        void Initialize(GachaDbContext context);
    }
}
