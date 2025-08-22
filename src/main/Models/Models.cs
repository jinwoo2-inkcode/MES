namespace main.Models;

public class Models { }

public class Load
{
    public string? PRODUCT_ID { get; set; }
    public string? PRODUCT_DESC { get; set; }
    public string? DEMAND_ORD_NBR { get; set; }
    public double? LOAD_QTY { get; set; }
    public DateTime? CREATE_DATETIME { get; set; }
    public string? LOAD_ID { get; set; }
    public int? ACTIVE { get; set; }

}

public class Production
{
    public string? PRODUCT_ID { get; set; }
    public string? PRODUCT_DESC { get; set; }
    public string? DEMAND_ORD_NBR { get; set; }
}