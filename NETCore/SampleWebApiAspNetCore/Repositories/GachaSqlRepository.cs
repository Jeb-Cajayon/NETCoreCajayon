using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class GachaSqlRepository : IGachaRepository
    {
        private readonly GachaDbContext _rngDbContext;

        public GachaSqlRepository(GachaDbContext rngDbContext)
        {
            _rngDbContext = rngDbContext;
        }

        public GachaEntity GetSingle(int id)
        {
            return _rngDbContext.GachaItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(GachaEntity item)
        {
            _rngDbContext.GachaItems.Add(item);
        }

        public void Delete(int id)
        {
            GachaEntity rngItem = GetSingle(id);
            _rngDbContext.GachaItems.Remove(rngItem);
        }

        public GachaEntity Update(int id, GachaEntity item)
        {
            _rngDbContext.GachaItems.Update(item);
            return item;
        }

        public IQueryable<GachaEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<GachaEntity> _allItems = _rngDbContext.GachaItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Number.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _rngDbContext.GachaItems.Count();
        }

        public bool Save()
        {
            return (_rngDbContext.SaveChanges() >= 0);
        }

        public ICollection<GachaEntity> GetRandomNum()
        {
            List<GachaEntity> toReturn = new List<GachaEntity>();

            toReturn.Add(GetRandomItem("Starter"));
            toReturn.Add(GetRandomItem("Main"));
            toReturn.Add(GetRandomItem("Dessert"));

            return toReturn;
        }

        private GachaEntity GetRandomItem(string type)
        {
            return _rngDbContext.GachaItems
                .Where(x => x.Type == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }
    }
}
