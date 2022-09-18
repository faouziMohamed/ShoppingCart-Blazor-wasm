using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public interface IShoppingCartService
{
  Task<IEnumerable<CartItemDto>?> GetItemsAsync(int userId);
  Task<CartItemDto?> AddItemToCartAsync(CartItemToAddDto cartItemToAddDto);
}
