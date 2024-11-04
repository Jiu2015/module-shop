using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Infrastructure;
using Shop.Infrastructure.Data;
using Shop.Module.Catalog.Entities;
using Shop.Module.Core.Extensions;

namespace Shop.Module.Catalog.Controllers
{
    /// <summary>
    /// 最近浏览API控制器，负责管理用户的最近浏览商品记录。
    /// </summary>
    [Authorize()]
    [Route("api/recently-viewed")]
    public class RecentlyViewedApiController : ControllerBase
    {
        private readonly IRepository<ProductRecentlyViewed> _productRecentlyViewedRepository;
        private readonly IWorkContext _workContext;

        public RecentlyViewedApiController(
            IRepository<ProductRecentlyViewed> productRecentlyViewedRepository,
            IWorkContext workContext)
        {
            _productRecentlyViewedRepository = productRecentlyViewedRepository;
            _workContext = workContext;
        }

        /// <summary>
        /// 获取用户最近浏览的商品列表。
        /// </summary>
        /// <param name="take">返回记录的数量，默认为20。</param>
        /// <returns>最近浏览的商品列表。</returns>
        [HttpGet()]
        public async Task<Result> List(int take = 20)
        {
        }

        /// <summary>
        /// 清除用户的所有最近浏览记录。
        /// </summary>
        /// <returns>操作结果。</returns>
        [HttpDelete()]
        public async Task<Result> Clear()
        {
            var user = await _workContext.GetCurrentUserAsync();
            var list = await _productRecentlyViewedRepository.Query()
                .Where(c => c.CustomerId == user.Id)
                .ToListAsync();
            foreach (var item in list)
            {
                item.IsDeleted = true;
            }
            await _productRecentlyViewedRepository.SaveChangesAsync();
            return Result.Ok();
        }

        /// <summary>
        /// 从用户的最近浏览记录中移除指定的商品。
        /// </summary>
        /// <param name="productId">需要移除的商品ID。</param>
        /// <returns>操作结果。</returns>
        [HttpDelete("{productId:int:min(1)}")]
        public async Task<Result> Remove(int productId)
        {
            var user = await _workContext.GetCurrentOrThrowAsync();
            var model = await _productRecentlyViewedRepository.Query()
                .Where(c => c.CustomerId == user.Id && c.ProductId == productId)
                .FirstOrDefaultAsync();
            if (model != null)
            {
                model.IsDeleted = true;
                await _productRecentlyViewedRepository.SaveChangesAsync();
            }
            return Result.Ok();
        }
    }
}
