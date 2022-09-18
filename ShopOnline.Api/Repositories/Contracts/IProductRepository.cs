using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts;

public interface IProductRepository
{
  Task<IEnumerable<Product?>> GetItemsAsync();
  Task<IEnumerable<ProductCategory?>> GetCategoriesAsync();
  Task<Product?> GetItemByIdAsync(int id);
  Task<ProductCategory?> GetCategoryByIdAsync(int id);
}
