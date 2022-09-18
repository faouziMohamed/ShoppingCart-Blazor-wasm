using ShopOnline.Api.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.Api.Extensions;

static public class DtoConversions
{
  static public IEnumerable<ProductDto> ConverterDto(this IEnumerable<Product> products,
    IEnumerable<ProductCategory> productCategories)
  {
    return (
        from product in products
        join productCategory in productCategories on product.CategoryId equals productCategory.Id
        select product.ConverterDto(productCategory)
      )
      .ToList(); // ProductDto(product, productCategory)).ToList();
  }
  static public ProductDto ConverterDto(this Product product, ProductCategory productCategory)
  {
    return new ProductDto
    {
      Id = product.Id,
      Name = product.Name,
      Description = product.Description,
      ImageURL = product.ImageURL,
      Price = product.Price,
      Qty = product.Qty,
      CategoryId = product.CategoryId,
      CategoryName = productCategory.Name
    };
  }

  static public IEnumerable<CartItemDto> ConverterDto(this IEnumerable<CartItem> cartItems,
    IEnumerable<Product> products)
  {
    return (
        from cartItem in cartItems
        join product in products on cartItem.ProductId equals product.Id
        select cartItem.ConverterDto(product))
      .ToList();
  }

  static public CartItemDto ConverterDto(this CartItem cartItem, Product product)
  {
    return new CartItemDto
    {
      Id = cartItem.Id,
      ProductId = cartItem.ProductId,
      ProductName = product.Name,
      ProductDescription = product.Description,
      ProductImageURL = product.ImageURL,
      Price = product.Price,
      Qty = product.Qty,
      TotalPrice = product.Price*cartItem.Qty
    };
  }
}
