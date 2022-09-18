using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8618
namespace ShopOnline.Api.Entities;

[SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
public sealed class User
{
  public int Id { get; set; }
  public string UserName { get; set; }
}
