using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IShoppingCartService
{
  Task<List<CartItemDto>?> GetItemsAsync(int userId);
  Task<CartItemDto?> AddItemToCartAsync(CartItemToAddDto cartItemToAddDto);
  Task<CartItemDto?> DeleteItemAsync(int cartId);
}
