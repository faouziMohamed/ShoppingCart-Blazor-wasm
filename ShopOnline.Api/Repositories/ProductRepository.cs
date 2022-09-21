using Microsoft.EntityFrameworkCore;
using ShopOnline.Api.Data;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Repositories.Contracts;

namespace ShopOnline.Api.Repositories;

public sealed class ProductRepository : IProductRepository
{
  private readonly ShopOnlineDbContext _context;
  public ProductRepository(ShopOnlineDbContext context)
  {
    _context = context;
  }
  public async Task<List<Product>> GetItemsAsync()
  {
    return await _context.Products.Include(static p => p.ProductCategory).ToListAsync();
  }
  public async Task<List<ProductCategory>> GetCategoriesAsync()
  {
    return await _context.ProductCategories.ToListAsync();
  }
  public async Task<Product?> GetItemByIdAsync(int id)
  {
    return await _context.Products.Include(p => p.ProductCategory).FirstOrDefaultAsync(p => p.Id == id);
  }
  public async Task<ProductCategory?> GetCategoryByIdAsync(int id)
  {
    return await _context.ProductCategories.SingleOrDefaultAsync(c => c.Id == id);
  }
  public async Task<List<Product>> GetItemsByCategoryAsync(int categoryId)
  {
    return await _context.Products
      .Where(p => p.CategoryId == categoryId)
      .Include(p => p.ProductCategory)
      .ToListAsync();
  }
}
