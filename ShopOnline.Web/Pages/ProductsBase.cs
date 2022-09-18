using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductsBase : ComponentBase
{
  [Inject]
  public IProductService ProductService { get; set; }

  protected IEnumerable<ProductDto>? Products { get; set; }

  protected async override Task OnInitializedAsync()
  {
    Products = await ProductService.GetItemsAsync();
  }
  protected IOrderedEnumerable<IGrouping<int, ProductDto>> GetGroupedProductsByCategory()
  {
    return from product in Products
      group product by product.CategoryId
      into prodCatGroup
      orderby prodCatGroup.Key
      select prodCatGroup;
  }

  protected string GetCategoryName(IGrouping<int, ProductDto> groupedProductsDtos)
  {
    return groupedProductsDtos
      .FirstOrDefault(pg => pg.CategoryId == groupedProductsDtos.Key)
      .CategoryName;
  }
}
