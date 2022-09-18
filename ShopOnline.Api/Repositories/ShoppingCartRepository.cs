using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Repositories;

public sealed class ShoppingCartRepository : IShoppingCartRepository
{
  private readonly ShopOnlineDbContext _context;
  public ShoppingCartRepository(ShopOnlineDbContext context)
  {
    _context = context;
  }
  public async Task<CartItem?> AddItem(CartItemToAddDto itemToAddDto)
  {
    if (await CardItemExists(itemToAddDto.CartId, itemToAddDto.ProductId))
    {
      return default;
    }

    var item = await (from product in _context.Products
      where product.Id == itemToAddDto.ProductId
      select new CartItem
      {
        CartId = itemToAddDto.CartId,
        ProductId = product.Id,
        Qty = itemToAddDto.Qty
      }).SingleOrDefaultAsync();

    if (item == null)
    {
      return default;
    }

    EntityEntry<CartItem> result = await _context.CartItems.AddAsync(item);
    await _context.SaveChangesAsync();
    return result.Entity;
  }
  public async Task<CartItem> UpdateQty(int id, CartItemQtyUpdateDto itemQtyUpdateDto)
  {
    throw new NotImplementedException();
  }
  public async Task<CartItem> DeleteItem(int id)
  {
    throw new NotImplementedException();
  }
  public async Task<CartItem?> GetItem(int id)
  {
    return await (from cart in _context.Carts
      join cartItem in _context.CartItems on cart.Id equals cartItem.CartId
      where cartItem.Id == id
      select new CartItem
      {
        Id = cartItem.Id,
        ProductId = cartItem.ProductId,
        Qty = cartItem.Qty,
        CartId = cartItem.CartId
      }).SingleOrDefaultAsync();
  }
  public async Task<List<CartItem>> GetItems(int userId)
  {
    return await (from cart in _context.Carts
      join cartItem in _context.CartItems on cart.Id equals cartItem.CartId
      where cart.UserId == userId
      select new CartItem
      {
        Id = cartItem.Id,
        ProductId = cartItem.ProductId,
        CartId = cartItem.CartId,
        Qty = cartItem.Qty
      }).ToListAsync();
  }
  private async Task<bool> CardItemExists(int cartId, int productId)
  {
    return await _context
      .CartItems
      .AnyAsync(item => item.CartId == cartId && item.ProductId == productId);
  }
}
