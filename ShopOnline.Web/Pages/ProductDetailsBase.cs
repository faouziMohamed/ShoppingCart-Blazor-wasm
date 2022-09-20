using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class ProductDetailsBase : ComponentBase
{
  [Parameter]
  public int Id { get; set; }

  [Inject]
  public IProductService ProductService { get; set; }

  [Inject]
  public IShoppingCartService ShoppingCartService { get; set; }

  protected ProductDto Product { get; set; }
  protected string ErrorMessage { get; set; }

  [Inject]
  public NavigationManager NavigationManager { get; set; }

  protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
  {
    try
    {
      var cartItemDto = await ShoppingCartService.AddItemToCartAsync(cartItemToAddDto);
      NavigationManager.NavigateTo("/ShoppingCart");
    }
    catch (Exception e)
    {
      ErrorMessage = e.Message;
    }


  }
  protected async override Task OnInitializedAsync()
  {
    try
    {
      Product = await ProductService.GetItemAsync(Id);
    }
    catch (Exception ex)
    {
      ErrorMessage = ex.Message;
    }
  }
}
