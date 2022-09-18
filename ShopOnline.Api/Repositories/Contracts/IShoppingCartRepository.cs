using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories.Contracts;

public interface IShoppingCartRepository
{
  Task<CartItem?> AddItem(CartItemToAddDto itemToAddDto);
  Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto itemQtyUpdateDto);
  Task<CartItem> DeleteItem(int id);
  Task<CartItem?> GetItem(int id);
  Task<List<CartItem>> GetItems(int userId);
}
