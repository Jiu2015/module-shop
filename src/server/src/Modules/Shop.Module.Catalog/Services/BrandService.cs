using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.Infrastructure.Web.StandardTable;
using Shop.Module.Catalog.Data;
using Shop.Module.Catalog.Entities;
using Shop.Module.Catalog.ViewModels;
using Shop.Module.Core.Cache;

namespace Shop.Module.Catalog.Services
{
    public class BrandService : IBrandService
    {
        private readonly string _key = CatalogKeys.BrandAll;

        private readonly IRepository<Brand> _brandRepository;
        private readonly IStaticCacheManager _cache;

        public BrandService(IRepository<Brand> brandRepository,
            IStaticCacheManager cache)
        {
            _brandRepository = brandRepository;
            _cache = cache;
        }

        public async Task<IList<Brand>> GetAllByCache()
        {
            var result = await _cache.GetAsync(_key, async () =>
            {
                return await _brandRepository.Query().ToListAsync();
            });
            return result;
        }

        public async Task Create(Brand brand)
        {
            using (var transaction = _brandRepository.BeginTransaction())
            {
                _brandRepository.Add(brand);
                await _brandRepository.SaveChangesAsync();
                transaction.Commit();
            }
            await ClearCache();
        }

        public async Task Update(Brand brand)
        {
            await _brandRepository.SaveChangesAsync();
            await ClearCache();
        }

        public async Task<Result<StandardTableResult<BrandResult>>> List(StandardTableParam param)
        {
        }

        public async Task ClearCache()
        {
            await Task.Run(() =>
            {
                _cache.Remove(_key);
            });
        }
    }
}
