using System.Net;
using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Services;

public sealed class ProductService : IProductService
{
  private readonly HttpClient _httpClient;
  public ProductService(HttpClient httpClient)
  {
    _httpClient = httpClient;

  }
  public async Task<List<ProductDto>> GetItemsAsync()
  {
    try
    {
      var response = await _httpClient.GetAsync("api/product");

      if (response.IsSuccessStatusCode)
      {
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
          return new List<ProductDto>();
        }

        var productsDto = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        return productsDto ?? new List<ProductDto>();
      }

      string message = await response.Content.ReadAsStringAsync();
      throw new Exception(message);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
  public async Task<ProductDto?> GetItemAsync(int id)
  {
    var response = await _httpClient.GetAsync($"api/Product/{id}");

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        return default;
      }

      return await response.Content.ReadFromJsonAsync<ProductDto>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new Exception(message);
  }
  public async Task<List<ProductCategoryDto>> GetProductCategoriesAsync()
  {
    var response = await _httpClient.GetAsync("api/Product/GetProductCategories");

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        return new List<ProductCategoryDto>();
      }

      var productCategories = await response.Content.ReadFromJsonAsync<List<ProductCategoryDto>>();
      return productCategories ?? new List<ProductCategoryDto>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new Exception($"Http Status Code: {response.StatusCode} Message:  {message}");
  }
  public async Task<List<ProductDto>> GetProductsByCategoryAsync(int categoryId)
  {
    var response = await _httpClient.GetAsync($"api/Product/{categoryId}/GetItemsByCategory");

    if (response.IsSuccessStatusCode)
    {
      if (response.StatusCode == HttpStatusCode.NoContent)
      {
        return new List<ProductDto>();
      }

      var products = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
      return products ?? new List<ProductDto>();
    }

    string message = await response.Content.ReadAsStringAsync();
    throw new Exception($"Http Status Code: {response.StatusCode} Message:  {message}");
  }
}
