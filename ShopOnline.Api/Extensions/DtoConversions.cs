using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions;

static public class DtoConversions
{
  static public IEnumerable<ProductDto> ConverterToDto(this IEnumerable<Product> products)
  {
    return (from product in products select product.ConverterToDto()).ToList();
  }
  static public ProductDto ConverterToDto(this Product product)
  {
    return new ProductDto
    {
      Id = product.Id,
      Name = product.Name,
      Description = product.Description,
      ImageURL = product.ImageURL,
      Price = product.Price,
      Qty = product.Qty,
      CategoryId = product.ProductCategory.Id,
      CategoryName = product.ProductCategory.Name
    };
  }

  static public IEnumerable<CartItemDto> ConverterToDto(this IEnumerable<CartItem> cartItems,
    IEnumerable<Product> products)
  {
    return (
        from cartItem in cartItems
        join product in products on cartItem.ProductId equals product.Id
        select cartItem.ConverterToDto(product))
      .ToList();
  }

  static public CartItemDto ConverterToDto(this CartItem cartItem, Product product)
  {
    return new CartItemDto
    {
      Id = cartItem.Id,
      ProductId = cartItem.ProductId,
      ProductName = product.Name,
      ProductDescription = product.Description,
      ProductImageURL = product.ImageURL,
      Price = product.Price,
      CartId = cartItem.Id,
      Qty = cartItem.Qty,
      TotalPrice = product.Price*cartItem.Qty
    };
  }

  static public List<ProductCategoryDto> ConverterToDto(this IEnumerable<ProductCategory> productCategories)
  {
    return (from category in productCategories select category.ConvertToDto()).ToList();
  }
  static public ProductCategoryDto ConvertToDto(this ProductCategory category)
  {

    return new ProductCategoryDto
    {
      Id = category.Id,
      Name = category.Name,
      IconCSS = category.IconCSS
    };
  }
}
