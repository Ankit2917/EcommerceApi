namespace EcommerceApi.Model
{
    public class ProductDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public string colour { get; set; }
        public string description { get; set; }
        public IFormFile? imageurl { get; set; }
        public string price { get; set; }

    }
}
