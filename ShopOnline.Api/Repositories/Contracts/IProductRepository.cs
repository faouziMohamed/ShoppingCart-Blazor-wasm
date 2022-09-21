using ShopOnline.Api.Entities;

namespace ShopOnline.Api.Repositories.Contracts;

public interface IProductRepository
{
  Task<List<Product>> GetItemsAsync();
  Task<List<ProductCategory>> GetCategoriesAsync();
  Task<Product?> GetItemByIdAsync(int id);
  Task<ProductCategory?> GetCategoryByIdAsync(int id);
  Task<List<Product>> GetItemsByCategoryAsync(int categoryId);
}
