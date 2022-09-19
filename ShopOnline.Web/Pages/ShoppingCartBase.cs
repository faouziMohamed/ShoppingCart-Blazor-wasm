using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web;
using ShopOnline.Web.Services.Contracts;

// ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

public abstract class ShoppingCartBase : ComponentBase
{

  [Inject]
  public IShoppingCartService ShoppingCartService { get; set; } = default!;

  protected List<CartItemDto>? ShoppingCartItems { get; set; }
  protected string ErrorMessage { get; set; } = default!;
  protected string TotalPrice { get; set; } = default!;
  protected int TotalQuantity { get; set; }

  [Inject]
  public IJSRuntime Js { get; set; }

  protected async override Task OnInitializedAsync()
  {
    try
    {
      ShoppingCartItems = await ShoppingCartService.GetItemsAsync(HardCoded.UserId);
      CartChanged();
    }
    catch (Exception e)
    {
      ErrorMessage = e.Message;
    }
  }
  protected async Task UpdateQtyCartItem_click(int id, int qty)
  {
    try
    {
      if (qty > 0)
      {
        var updateItemDto = new CartItemQtyUpdateDto { CartItemId = id, Qty = qty };
        var returnedUpdateItemDto = await ShoppingCartService.UpdateItemQtyAsync(updateItemDto);
        Console.WriteLine(returnedUpdateItemDto.ToString());
        UpdateItemTotalPrice(returnedUpdateItemDto!);
        CartChanged();
        await MakeUpdateQtyButtonInvisible(id, false);
      }
      else
      {
        var item = ShoppingCartItems?.FirstOrDefault(i => i.Id == id);

        if (item != null)
        {
          item.Qty = 1;
          item.TotalPrice = item.Price;
        }
      }
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
    finally
    {
      StateHasChanged();
    }

  }

  protected async Task UpdateQty_Input(int id)
  {
    await MakeUpdateQtyButtonInvisible(id, true);
  }

  protected async Task MakeUpdateQtyButtonInvisible(int id, bool visible)
  {
    await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", id, visible);
  }

  private CartItemDto GetCartItem(int cartItemId)
  {
    return ShoppingCartItems!.FirstOrDefault(i => i.Id == cartItemId)!;
  }
  private void UpdateItemTotalPrice(CartItemDto cartItemDto)
  {
    var item = GetCartItem(cartItemDto.Id);

    if (item == null) return;
    item.TotalPrice = cartItemDto.Price*cartItemDto.Qty;
  }

  protected async Task DeleteCartItem_Click(int cartItemId)
  {
    var cartItemDto = await ShoppingCartService.DeleteItemAsync(cartItemId);
    RemoveCartItem(cartItemId);
    CartChanged();
    StateHasChanged();
  }


  private void RemoveCartItem(int cartItemId)
  {
    var cartItemDto = GetCartItem(cartItemId);
    ShoppingCartItems.Remove(cartItemDto);
  }

  private void SetTotalPrice()
  {
    TotalPrice = ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
  }
  private void SetTotalQuantity()
  {
    TotalQuantity = ShoppingCartItems.Sum(q => q.Qty);
  }

  private void CalculateCartSummaryTotals()
  {
    SetTotalPrice();
    SetTotalQuantity();
  }

  private void CartChanged()
  {
    CalculateCartSummaryTotals();
    ShoppingCartService.RaiseEventOnShoppingCartChanged(TotalQuantity);
  }
}
