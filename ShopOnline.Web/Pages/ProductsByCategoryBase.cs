using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductsByCategoryBase : ComponentBase

{
  [Inject] public IProductService ProductService { get; set; }
  [Parameter] public int CategoryId { get; set; }

  protected string CategoryName { get; set; }
  protected List<ProductDto> Products { get; set; } = new();
  protected string? ErrorMessage { get; set; }

  [Inject]
  public IManageProductLocalStorageService ManageProductLocalStorageService { get; set; }

  private async Task<List<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
  {
    List<ProductDto> productCollection = await ManageProductLocalStorageService.GetCollectionAsync();

    if (productCollection.Any())
    {
      return productCollection.Where(p => p.CategoryId == categoryId).ToList();
    }

    return await ProductService.GetProductsByCategoryAsync(categoryId);
  }
  protected async override Task OnParametersSetAsync()
  {
    try
    {
      Products = await GetProductCollectionByCategoryId(CategoryId);

      if (Products.Any())
      {
        var productDto = Products.FirstOrDefault(p => p.CategoryId == CategoryId);

        if (productDto != null)
        {
          CategoryName = productDto.CategoryName;
        }
      }
    }
    catch (Exception e)
    {
      ErrorMessage = e.Message;
    }
  }
}
