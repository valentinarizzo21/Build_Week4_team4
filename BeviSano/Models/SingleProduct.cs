namespace BeviSano.Models
{
    public class SingleProduct
    {
            public Guid Id_Product { get; set; }
            public string Name_Product { get; set; }
            public decimal Price_Product { get; set; }
            public int Stock_Product { get; set; }
            public string Seller_Product { get; set; }
            public decimal Sale_Product { get; set; }
            public int Arrival_Date_Product { get; set; }
            public string Cover_Product { get; set; }
            public int Id_Category { get; set; }
            public string Description_Product { get; set; }
        }


    }
