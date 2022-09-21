using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IManageCartItemsLocalStorageService
{
  Task<List<CartItemDto>> GetCollectionAsync();
  Task SaveCollectionAsync(List<CartItemDto> cartItems);
  Task RemoveCollectionAsync();
}
