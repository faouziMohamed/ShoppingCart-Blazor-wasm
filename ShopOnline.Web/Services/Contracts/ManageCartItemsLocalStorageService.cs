using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public sealed class ManageCartItemsLocalStorageService : IManageCartItemsLocalStorageService
{
  private const string Key = "CartItemsCollection";
  private readonly ILocalStorageService _localStorageService;
  private readonly IShoppingCartService _shoppingCartService;
  public ManageCartItemsLocalStorageService(ILocalStorageService localStorageService, IShoppingCartService shoppingCartService)
  {
    _localStorageService = localStorageService;
    _shoppingCartService = shoppingCartService;

  }
  public async Task<List<CartItemDto>> GetCollectionAsync()
  {
    return await _localStorageService.GetItemAsync<List<CartItemDto>>(Key) ?? await AddCollectionAsync();
  }
  public async Task SaveCollectionAsync(List<CartItemDto> cartItems)
  {
    await _localStorageService.SetItemAsync(Key, cartItems);
  }
  public async Task RemoveCollectionAsync()
  {
    await _localStorageService.RemoveItemAsync(Key);
  }

  private async Task<List<CartItemDto>> AddCollectionAsync()
  {
    List<CartItemDto> cartItems = await _shoppingCartService.GetItemsAsync(HardCoded.UserId);

    if (!cartItems.Any()) return cartItems;
    await _localStorageService.SetItemAsync(Key, cartItems);
    return cartItems;
  }
}
