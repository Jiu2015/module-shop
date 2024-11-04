using Shop.Module.Catalog.Entities;
using Shop.Module.Catalog.ViewModels;

namespace Shop.Module.Catalog.Services
{
    public class ProductPricingService : IProductPricingService
    {
        public CalculatedProductPrice CalculateProductPrice(GoodsListResult productThumbnail)
        {
            return CalculateProductPrice(productThumbnail.Price, productThumbnail.OldPrice, productThumbnail.SpecialPrice, productThumbnail.SpecialPriceStart, productThumbnail.SpecialPriceEnd);
        }

        public CalculatedProductPrice CalculateProductPrice(Product product)
        {
            return CalculateProductPrice(product.Price, product.OldPrice, product.SpecialPrice, product.SpecialPriceStart, product.SpecialPriceEnd);
        }

        public CalculatedProductPrice CalculateProductPrice(decimal price, decimal? oldPrice, decimal? specialPrice, DateTime? specialPriceStart, DateTime? specialPriceEnd)
        {
        }
    }
}
