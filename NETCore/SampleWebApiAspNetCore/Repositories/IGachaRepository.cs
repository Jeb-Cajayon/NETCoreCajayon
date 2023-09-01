using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IGachaRepository
    {
        GachaEntity GetSingle(int id);
        void Add(GachaEntity item);
        void Delete(int id);
        GachaEntity Update(int id, GachaEntity item);
        IQueryable<GachaEntity> GetAll(QueryParameters queryParameters);
        ICollection<GachaEntity> GetRandomNum();
        int Count();
        bool Save();
    }
}
