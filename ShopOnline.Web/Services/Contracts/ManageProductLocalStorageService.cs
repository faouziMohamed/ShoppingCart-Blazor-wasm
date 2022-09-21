using Blazored.LocalStorage;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Web.Services.Contracts;

public sealed class ManageProductLocalStorageService : IManageProductLocalStorageService
{
  private const string Key = "ProductCollection";
  private readonly ILocalStorageService _localStorageService;
  private readonly IProductService _productService;
  public ManageProductLocalStorageService(ILocalStorageService localStorageService, IProductService productService)
  {
    _localStorageService = localStorageService;
    _productService = productService;
  }
  public async Task<List<ProductDto>> GetCollectionAsync()
  {
    return await _localStorageService.GetItemAsync<List<ProductDto>>(Key) ?? await AddCollectionAsync();
  }
  public async Task RemoveFromCollectionAsync()
  {
    await _localStorageService.RemoveItemAsync(Key);
  }
  private async Task<List<ProductDto>> AddCollectionAsync()
  {
    List<ProductDto> productCollection = await _productService.GetItemsAsync();

    if (!productCollection.Any())
    {
      await _localStorageService.SetItemAsync(Key, productCollection);
    }

    return productCollection;
  }
}
