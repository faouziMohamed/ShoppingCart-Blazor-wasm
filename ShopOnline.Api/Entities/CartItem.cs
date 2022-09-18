namespace ShopOnline.Api.Entities;

public sealed class CartItem
{
  public int Id { get; set; }
  public int CartId { get; set; }
  public int ProductId { get; set; }
  public int Qty { get; set; }

  public Cart Cart { get; set; }
  public Product Product { get; set; }
}
