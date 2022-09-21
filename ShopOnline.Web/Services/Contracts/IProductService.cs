using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IProductService
{
  Task<IEnumerable<ProductDto>> GetItemsAsync();
  Task<ProductDto?> GetItemAsync(int id);
  Task<List<ProductCategoryDto>> GetProductCategoriesAsync();
  Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId);
}
