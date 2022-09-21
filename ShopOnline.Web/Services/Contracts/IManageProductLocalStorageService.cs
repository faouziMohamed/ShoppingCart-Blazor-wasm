using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IManageProductLocalStorageService
{
  Task<List<ProductDto>> GetCollectionAsync();
  Task RemoveFromCollectionAsync();
}
