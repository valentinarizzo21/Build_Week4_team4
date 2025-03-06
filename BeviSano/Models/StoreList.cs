namespace BeviSano.Models
{
    public class StoreList
    {
        public List<Product> Products { get; set; } = [];

        public List<Category>? Categories { get; set; } = [];
    }
}
