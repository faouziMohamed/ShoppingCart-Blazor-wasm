using Microsoft.AspNetCore.Mvc;
using ShopOnline.Api.Entities;
using ShopOnline.Api.Extensions;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Controllers;

[Route("api/[controller]")]
public class ShoppingCartController : ControllerBase
{
  private readonly IProductRepository _productRepository;
  private readonly IShoppingCartRepository _shoppingCartRepository;
  public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
  {
    _shoppingCartRepository = shoppingCartRepository;
    _productRepository = productRepository;
  }

  [HttpGet("{userId:int}/GetItems")]
  public async Task<ActionResult<IEnumerable<CartItemDto>>> GetItems(int userId)
  {
    try
    {
      List<CartItem>? cartItems = await _shoppingCartRepository.GetItems(userId);

      if (cartItems == null)
      {
        return NoContent();
      }

      IEnumerable<Product?>? products = await _productRepository.GetItemsAsync();
      if (products == null) throw new Exception("No products exist in the system");
      return Ok(cartItems.ConverterDto(products));
    }
    catch (Exception e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
  }

  [HttpPost("{id:int}")]
  public async Task<ActionResult<CartItemDto>> GetItem(int id)
  {
    try
    {
      var cartItem = await _shoppingCartRepository.GetItem(id);

      if (cartItem == null)
      {
        return NotFound();
      }

      var product = await _productRepository.GetItemByIdAsync(cartItem.ProductId);
      if (product == null) throw new Exception("No products exist in the system");

      return Ok(cartItem.ConverterDto(product));

    }
    catch (Exception e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
  }

  [HttpPost]
  public async Task<ActionResult<CartItemDto>> AddItem([FromBody] CartItemToAddDto itemToAddDto)
  {
    try
    {
      var newCartItem = await _shoppingCartRepository.AddItem(itemToAddDto);

      if (newCartItem == null)
      {
        return NoContent();
      }

      var product = await _productRepository.GetItemByIdAsync(newCartItem.ProductId);

      if (product == null)
      {
        throw new Exception($"Something wen wrong when attempting to retrieve product (productId: {itemToAddDto.ProductId})");
      }

      var newCartItemDto = newCartItem.ConverterDto(product);
      return CreatedAtAction(actionName: nameof(GetItem), routeValues: new { id = newCartItemDto.Id }, newCartItemDto);
    }
    catch (Exception e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
  }
  [HttpDelete("{id:int}")]
  public async Task<ActionResult> DeleteItem(int id)
  {
    try
    {
      var cartItem = await _shoppingCartRepository.DeleteItem(id);

      if (cartItem == null)
      {
        return NotFound();
      }

      var product = await _productRepository.GetItemByIdAsync(cartItem.ProductId);
      if (product == null) throw new Exception("No products exist in the system");
      return Ok(cartItem.ConverterDto(product));

    }
    catch (Exception e)
    {
      return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
    }
  }
}
