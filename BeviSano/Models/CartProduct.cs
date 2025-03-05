namespace BeviSano.Models
{
    public class CartProduct
    {
        public Guid Id_Product { get; set; }
        public string Name_Product { get; set; }
        public decimal Price_Product { get; set; }
        public int Quantity_Product { get; set; }
        public decimal Sale_Product { get; set; }
        public int Arrival_Date_Product { get; set; }
        public string Cover_Product { get; set; }
    }
}
