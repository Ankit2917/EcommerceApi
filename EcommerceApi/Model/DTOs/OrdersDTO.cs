namespace EcommerceApi.Model.DTOs
{
    public class OrdersDTO
    {
        public int id { get; set; }

        public string name { get; set; }
        public string price { get; set; }
        public string Address { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public string pincode { get; set; }
        public string phonenumber { get; set; }
        public string email { get; set; }
        public int Userid { get; set; }
        public User user { get; set; }

        
    }
}
