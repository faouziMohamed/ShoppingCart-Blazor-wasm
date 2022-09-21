using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductsBase : ComponentBase
{
  [Inject]
  public IProductService ProductService { get; set; }

  [Inject]
  public IShoppingCartService ShoppingCartService { get; set; }

  [Inject]
  public IManageProductLocalStorageService ManageProductLocalStorageService { get; set; }

  [Inject]
  public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

  protected IEnumerable<ProductDto>? Products { get; set; }
  protected string? ErrorMessage { get; set; }

  protected async override Task OnInitializedAsync()
  {
    try
    {
      await ClearLocalStorage();
      Products = await ManageProductLocalStorageService.GetCollectionAsync();
      List<CartItemDto> shoppingCart = await ManageCartItemsLocalStorageService.GetCollectionAsync();
      int totalQty = shoppingCart.Sum(static x => x.Qty);
      ShoppingCartService.RaiseEventOnShoppingCartChanged(totalQty);
    }
    catch (Exception e)
    {
      ErrorMessage = e.Message;
    }
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
      .FirstOrDefault(pg => pg.CategoryId == groupedProductsDtos.Key)!
      .CategoryName;
  }

  private async Task ClearLocalStorage()
  {
    await ManageProductLocalStorageService.RemoveFromCollectionAsync();
    await ManageCartItemsLocalStorageService.RemoveCollectionAsync();
  }
}
