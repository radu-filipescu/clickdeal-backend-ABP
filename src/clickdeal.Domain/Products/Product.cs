using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Nodes;
using Volo.Abp.Domain.Entities.Auditing;

namespace clickdeal.Products;

public class Product : AuditedAggregateRoot<Guid>
{

    public string CodIdentificareSmartbill { get; set; } = string.Empty;

    public string SmartbillProductName { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    // current price
    public double Price { get; set; }

    // if PriceDiscount > 0, it means that price was higher a while ago
    public double PriceDiscount { get; set; }

    public string DescriptionShort { get; set; } = string.Empty;

    public string DescriptionLong { get; set; } = string.Empty;

    public string Brand { get; set; } = string.Empty;

    // external URL for images src, separated by #
    public string PhotoPaths { get; set; } = string.Empty;

    // all the product's categories, separated by #
    public string Categories { get; set; } = string.Empty;

    // serialized JSON holding different characteristics of the product
    public string Specs { get; set; } = string.Empty;

    public int Quantity { get; set; }

    public bool VisibleOnWebsite { get; set; }

    public static bool AreSpecsEqual(string specs1, string specs2)
    {
        if (specs1 == string.Empty && specs2 == string.Empty)
            return true;
        
        var obj1 = JsonConvert.DeserializeObject<Dictionary<string, string>>(specs1);
        var obj2 = JsonConvert.DeserializeObject<Dictionary<string, string>>(specs2);

        if (obj1 == null || obj2 == null)
            return false;

        if (obj1.Count != obj2.Count)
            return false;

        foreach(KeyValuePair<string, string> entry in obj1)
        {
            if(!obj2.ContainsKey(entry.Key) || obj2.GetOrDefault(entry.Key) != entry.Value)
                return false;
        }

        foreach (KeyValuePair<string, string> entry in obj2)
        {
            if (!obj1.ContainsKey(entry.Key) || obj1.GetOrDefault(entry.Key) != entry.Value)
                return false;
        }

        return true;
    }
}