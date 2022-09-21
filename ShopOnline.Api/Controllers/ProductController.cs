using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
  private readonly IProductRepository _productRepository;
  public ProductController(IProductRepository productRepository)
  {
    _productRepository = productRepository;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerator<ProductDto>>> GetItems()
  {
    try
    {
      List<Product> products = await _productRepository.GetItemsAsync();
      if (!products.Any()) return NotFound();
      return Ok(products.ConverterToDto());
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database"
      );
    }
  }

  // get item by id
  [HttpGet("{id:int}")]
  public async Task<ActionResult<ProductDto>> GetItem(int id)
  {
    try
    {
      var product = await _productRepository.GetItemByIdAsync(id);
      if (product == null) return NotFound();
      return Ok(product.ConverterToDto());
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database"
      );
    }
  }

  // get all categories
  [HttpGet(nameof(GetProductCategories))]
  public async Task<ActionResult<IEnumerable<ProductCategory>>> GetProductCategories()
  {
    try
    {
      List<ProductCategory> productCategories = await _productRepository.GetCategoriesAsync();
      return Ok(productCategories.ConverterToDto());
    }
    catch (Exception)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database"
      );
    }
  }
// get items by category
  [HttpGet("{categoryId:int}/GetItemsByCategory")]
  public async Task<ActionResult<IEnumerable<ProductDto>>> GetItemsByCategory(int categoryId)
  {
    try
    {
      List<Product> products = await _productRepository.GetItemsByCategoryAsync(categoryId);
      List<ProductCategory> productCategories = await _productRepository.GetCategoriesAsync();

      if (!products.Any() || !productCategories.Any())
      {
        return NotFound();
      }

      IEnumerable<ProductDto> productDtos = products.ConverterToDto();
      return Ok(productDtos);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database"
      );
    }
  }
}
