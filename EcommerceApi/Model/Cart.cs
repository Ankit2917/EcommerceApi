namespace EcommerceApi.Model
{
    public class Cart
    {
        public int id { get; set; }
        public int Userid { get; set; }
        public User? User { get; set; }
        public int Productid { get; set; }
        public Product?  Product { get; set; }
        public int? Price { get; set; } 
        public int? Quantity { get; set; }
    }
}
