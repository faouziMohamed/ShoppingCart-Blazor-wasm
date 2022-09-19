using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web;
using ShopOnline.Web.Services.Contracts;

public abstract class ShoppingCartBase : ComponentBase
{
  [Inject]
  public IShoppingCartService ShoppingCartService { get; set; }

  protected List<CartItemDto>? ShoppingCartItems { get; set; }
  protected string ErrorMessage { get; set; }
  protected async Task DeleteCartItem_Click(int cartItemId)
  {
    var cartItemDto = await ShoppingCartService.DeleteItemAsync(cartItemId);
    RemoveCartItem(cartItemDto.Id);
  }

  private CartItemDto GetCartItem(int cartItemId)
  {
    return ShoppingCartItems.FirstOrDefault(x => x.Id == cartItemId);
  }

  private void RemoveCartItem(int cartItemId)
  {
    var cartItemDto = GetCartItem(cartItemId);
    ShoppingCartItems.Remove(cartItemDto);
  }
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
