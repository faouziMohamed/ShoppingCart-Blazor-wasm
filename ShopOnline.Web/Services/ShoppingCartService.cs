using System.Net;
using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

internal class ShoppingCartService : IShoppingCartService
{
  private readonly HttpClient _httpClient;
  public ShoppingCartService(HttpClient httpClient)
  {
    _httpClient = httpClient;

  }

  public async Task<CartItemDto?> AddItemToCartAsync(CartItemToAddDto cartItemToAddDto)
  {
    var response = await _httpClient.PostAsJsonAsync("api/shoppingcart", cartItemToAddDto);

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent) return default;
      return await response.Content.ReadFromJsonAsync<CartItemDto>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new ApplicationException($"Http status code: {response.StatusCode}, message -{message}");
  }

  public async Task<IEnumerable<CartItemDto>?> GetItemsAsync(int userId)
  {
    var response = await _httpClient.GetAsync($"api/shoppingCart/{userId}/GetItems");

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        return Enumerable.Empty<CartItemDto>();
      }

      return await response.Content.ReadFromJsonAsync<IEnumerable<CartItemDto>>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new ApplicationException($"Http status code: {response.StatusCode}, message -{message}");
  }
}
