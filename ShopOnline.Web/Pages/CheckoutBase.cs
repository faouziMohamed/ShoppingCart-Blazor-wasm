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

  protected IEnumerable<CartItemDto>? ShoppingCartItems { get; set; }
  protected int TotalQty { get; set; }
  protected string PaymentDescription { get; set; }
  protected decimal PaymentAmount { get; set; }
  protected async override Task OnInitializedAsync()
  {
    try
    {
      ShoppingCartItems = await ShoppingCartService.GetItemsAsync(HardCoded.UserId);

      if (ShoppingCartItems != null)
      {
        var orderId = Guid.NewGuid();
        PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
        TotalQty = ShoppingCartItems.Sum(p => p.Qty);
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
