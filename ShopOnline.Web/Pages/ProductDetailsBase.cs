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

  protected ProductDto? Product { get; set; }
  protected string ErrorMessage { get; set; }

  [Inject]
  public NavigationManager NavigationManager { get; set; }

  [Inject]
  public IManageProductLocalStorageService ManageProductLocalStorageService { get; set; }

  [Inject]
  public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

  public List<CartItemDto> ShoppingCartItems { get; set; }

  protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
  {
    try
    {
      var cartItemDto = await ShoppingCartService.AddItemToCartAsync(cartItemToAddDto);

      if (cartItemDto != null)
      {
        ShoppingCartItems.Add(cartItemDto);
        await ManageCartItemsLocalStorageService.SaveCollectionAsync(ShoppingCartItems);
      }

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
      ShoppingCartItems = await ManageCartItemsLocalStorageService.GetCollectionAsync();
      Product = await GetProductByIdAsync(Id);
    }
    catch (Exception ex)
    {
      ErrorMessage = ex.Message;
    }
  }
  private async Task<ProductDto?> GetProductByIdAsync(int id)
  {
    List<ProductDto> productDtos = await ManageProductLocalStorageService.GetCollectionAsync();
    return productDtos.FirstOrDefault(p => p.Id == id);
  }
}
