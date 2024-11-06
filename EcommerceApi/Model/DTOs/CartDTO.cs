namespace EcommerceApi.Model
{
    public class CartDTO
    {
     
            public int id { get; set; }
            public int Userid { get; set; }
            public User? user { get; set; }
            public int Productid { get; set; }
            public Product? product { get; set; }
            public int? Price { get; set; }
            public int? Quantity { get; set; }
         
    }
}
