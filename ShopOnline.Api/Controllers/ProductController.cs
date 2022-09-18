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
      IEnumerable<Product?> products = await _productRepository.GetItemsAsync();
      IEnumerable<ProductCategory?> productCategories = await _productRepository.GetCategoriesAsync();

      if (products == null || productCategories == null)
      {
        return NotFound();
      }

      IEnumerable<ProductDto> productDtos = products!.ConverterDto(productCategories!);
      return Ok(productDtos);
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

      if (product == null)
      {
        return NotFound();
      }

      var productCategories = await _productRepository.GetCategoryByIdAsync(product.CategoryId);
      var productDto = product.ConverterDto(productCategories);
      return Ok(productDto);
    }
    catch (Exception ex)
    {
      return StatusCode(StatusCodes.Status500InternalServerError,
        "Error retrieving data from the database"
      );
    }
  }
}
