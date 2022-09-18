using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web;
using ShopOnline.Web.Services.Contracts;

public abstract class ShoppingCartBase : ComponentBase
{
  [Inject]
  public IShoppingCartService ShoppingCartService { get; set; }

  public IEnumerable<CartItemDto>? ShoppingCartItems { get; set; }
  public string ErrorMessage { get; set; }
  protected async override Task OnInitializedAsync()
  {
    try
    {
      ShoppingCartItems = await ShoppingCartService.GetItemsAsync(HardCoded.UserId);
    }
    catch (Exception e)
    {
      ErrorMessage = e.Message;
    }
  }
}
