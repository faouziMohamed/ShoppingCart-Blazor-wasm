using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Shared;

public class ProductCategoriesNavMenuBase : ComponentBase

{
  [Inject]
  public IProductService ProductService { get; set; }

  protected List<ProductCategoryDto> ProductCategoryDtos { get; set; } = new();

  protected string? ErrorMessage { get; set; }

  protected async override Task OnInitializedAsync()
  {
    try
    {
      ProductCategoryDtos = await ProductService.GetProductCategoriesAsync();
    }
    catch (Exception ex)
    {
      ErrorMessage = ex.Message;
    }
  }
}
