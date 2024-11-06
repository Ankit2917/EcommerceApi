using EcommerceApi.Data;
using EcommerceApi.Data.IRepository;
using EcommerceApi.Model;
using EcommerceApi.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    [Route("api/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IRepository<Orders> _repository;
        private readonly ApplicationDbContext _context;

        public OrderController(IRepository<Orders> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<IActionResult> AddOrder(OrdersDTO ordersDTO)
        {
            if (ordersDTO != null)
            {
                Orders orders = new Orders();
                orders.name = ordersDTO.name;
                orders.state = ordersDTO.state;
                orders.pincode = ordersDTO.pincode;
                orders.city = ordersDTO.city;
                orders.country = ordersDTO.country;
                orders.Address = ordersDTO.Address;
                orders.email = ordersDTO.email;
                orders.Userid = ordersDTO.Userid;
                orders.phonenumber = ordersDTO.phonenumber;
                orders.price = ordersDTO.price;
                //await _repository.AddAsync(orders);
                await _context.orders.AddAsync(orders);
                await _context.SaveChangesAsync();
                return Ok(ordersDTO);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("GetAllOrder")]
        public IActionResult GetAllOrder(int id)
        {
            if (id != 0)
            {
                var list = _context.orders.Where(x => x.Userid == id);
                return Ok(list);
            }
            else return BadRequest("User id not presented");
        }


    }
}
