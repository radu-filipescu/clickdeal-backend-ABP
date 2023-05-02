using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace clickdeal.Products;

public class Product : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; } = string.Empty;

    public double Price { get; set; }

    public string Description { get; set; } = string.Empty;

    public string PhotoPath { get; set; } = string.Empty;

    public string PhotoPathSmall { get; set; } = string.Empty;

    // all the product's categories, separated by #
    public string Categories { get; set; } = string.Empty;

    // serialized JSON holding different characteristics of the product
    public string Specs { get; set; } = string.Empty;
}