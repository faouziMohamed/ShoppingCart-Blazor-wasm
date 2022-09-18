﻿using System.Net;
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
  public async Task<IEnumerable<ProductDto>?> GetItemsAsync()
  {
    try
    {
      var response = await _httpClient.GetAsync("api/product");

      if (response.IsSuccessStatusCode)
      {
        if (response.StatusCode == HttpStatusCode.NoContent)
        {
          return default;
        }

        return await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>();
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
}
