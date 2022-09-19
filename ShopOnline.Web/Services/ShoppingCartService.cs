using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
  public event Action<int> OnShoppingCartChanged;

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

  public async Task<List<CartItemDto>?> GetItemsAsync(int userId)
  {
    var response = await _httpClient.GetAsync($"api/shoppingCart/{userId}/GetItems");

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        return Enumerable.Empty<CartItemDto>().ToList();
      }

      return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new ApplicationException($"Http status code: {response.StatusCode}, message -{message}");
  }

  public async Task<CartItemDto?> DeleteItemAsync(int cartId)
  {
    var response = await _httpClient.DeleteAsync($"api/shoppingCart/{cartId}");

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent) return default;
      return await response.Content.ReadFromJsonAsync<CartItemDto>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new ApplicationException($"Http status code: {response.StatusCode}, message -{message}");

  }
  public async Task<CartItemDto?> UpdateItemQtyAsync(CartItemQtyUpdateDto cartItemToUpdateDto)
  {
    string jsonRequest = JsonSerializer.Serialize(cartItemToUpdateDto);
    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");
    var response = await _httpClient
      .PatchAsync(requestUri: $"api/shoppingCart/{cartItemToUpdateDto.CartItemId}", content);

    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<CartItemDto>();
    }

    return null;
  }
  public void RaiseEventOnShoppingCartChanged(int totalQty)
  {
    if (OnShoppingCartChanged != null)
    {
      OnShoppingCartChanged.Invoke(totalQty);
    }
  }
}
