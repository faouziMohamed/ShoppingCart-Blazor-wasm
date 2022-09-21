using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages;

public class CheckoutBase : ComponentBase
{
  [Inject]
  public IJSRuntime Js { get; set; }

  [Inject]
  public IShoppingCartService ShoppingCartService { get; set; }

  [Inject]
  public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

  protected IEnumerable<CartItemDto>? ShoppingCartItems { get; set; }
  protected int TotalQty { get; set; }
  protected string PaymentDescription { get; set; }
  protected decimal PaymentAmount { get; set; }
  protected async override Task OnInitializedAsync()
  {
    try
    {
      ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollectionAsync();

      if (ShoppingCartItems != null)
      {
        var orderId = Guid.NewGuid();
        PaymentAmount = ShoppingCartItems.Sum(static p => p.TotalPrice);
        TotalQty = ShoppingCartItems.Sum(static p => p.Qty);
        PaymentDescription = $"O_{HardCoded.UserId}_{orderId}";
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }

  protected async override Task OnAfterRenderAsync(bool firstRender)
  {
    try
    {
      if (firstRender)
      {
        await Js.InvokeVoidAsync("initPayPalButton");
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
}
