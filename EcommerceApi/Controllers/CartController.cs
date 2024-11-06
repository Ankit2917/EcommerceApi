using EcommerceApi.Data;
using EcommerceApi.Data.IRepository;
using EcommerceApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Controllers
{
    [Route("api/Cart")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IRepository<Cart> _repository;
        private readonly ApplicationDbContext _context;
        public CartController(IRepository<Cart> repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> AddCart(Cart cart)
        {
            await _repository.AddAsync(cart);
            return Ok(cart);
        }

        [HttpPost]
        [Route("LoginCart")]

        public async Task<IActionResult> LoginCart([FromBody] CartDTO Cart)
        {

            if (Cart != null)
            {

                var cartAlready = await _repository.FirstOrDefault(x => x.Productid == Cart.Productid);
                if (cartAlready != null)
                {
                    cartAlready.Quantity++;
                    cartAlready.Price = Cart.Quantity * Cart.Price;
                    _context.SaveChanges();
                }
                else
                {
                    Cart.Price = Cart.Quantity * Cart.Price;

                    Cart cartEntity = new Cart()
                    {
                        id = Cart.id,
                        Productid = Cart.Productid,
                        Quantity = Cart.Quantity,
                        Price = Cart.Price,
                        User = Cart.user,
                        Product = Cart.product

                    };
                    await _repository.AddAsync(cartEntity);
                }

                return Ok(Cart);

            }
            return Ok("Email and Password is Incorrect");
        }

        [HttpGet]
        [Route("GetUserCart")]
        public IActionResult GetUserCart(int UserId)
        {
            if (UserId != 0)
            {
                var UserCart = _context.cart.Where(x => x.Userid == UserId).Include(x => x.Product).Include(x => x.User).ToList();
                foreach (var item in UserCart)
                {
                    item.Product.imageurl = "https://localhost:7241/" + item.Product.imageurl;
                    continue;
                }
                return Ok(UserCart);
            }
            else
            {
                return BadRequest("UserId is null Found");
            }
        }

        [HttpDelete]
        [Route("DeletefromCart")]
        public async Task<IActionResult> DeletefromCart(int id, int userid)
        {
            if (id != 0)
            {
                var cartDetails = await _context.cart.FirstOrDefaultAsync(x => x.Userid == userid && x.id == id);
                if (cartDetails != null)
                {
                    _context.cart.Remove(cartDetails);
                return Ok(new { message = "Product Deleted Successfuly" }); 
                }
            }
            return BadRequest("Id or userId not received");
        }

        [HttpDelete]
        [Route("DeleteUserCartAll")]
        public async Task<IActionResult> DeleteUserCartAll(int id)
        {
            if (id != 0)
            {
                await _repository.DeleteAysncAll(x => x.Userid == id);
                return Ok(new { message = "Product Deleted Successfuly" });
            }
            return BadRequest();
        }

        [HttpGet]
        [Route("AddQuantityinCart")]
        public async Task<IActionResult> AddQuantityinCart(int id)
        {
            if (id != 0)
            {
                var cart = _context.cart.Include(x => x.Product).FirstOrDefault(x => x.id == id);
                if (cart.Quantity <= 20)
                {
                    cart.Quantity++;
                    cart.Price = cart.Quantity * Convert.ToInt32(cart.Product.Price);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Cart updated" });
                }
            }
            return BadRequest(new { message = "Cart updated" });

        }

        [HttpGet]
        [Route("SubtractQuantityinCart")]
        public async Task<IActionResult> SubtractQuantityinCart(int id)
        {
            if (id != 0)
            {
                var cart = _context.cart.Include(x => x.Product).FirstOrDefault(x => x.id == id);
                if (cart.Quantity > 1)
                {
                    cart.Quantity--;
                    cart.Price = cart.Quantity * Convert.ToInt32(cart.Product.Price);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Cart updated" });
                }
            }
            return BadRequest(new { message = "Cart updated" });

        }


        [HttpPost]
        [Route("AddtoCartDB")]

        public async Task<IActionResult> AddtoCartDB([FromBody] CartDTO Cart)
        {
            if (Cart != null)
            {

                var cartAlready = await _repository.FirstOrDefault(x => x.Productid == Cart.Productid && x.Userid == Cart.Userid);
                if (cartAlready != null)
                {
                    cartAlready.Quantity++;
                    cartAlready.Price = cartAlready.Quantity * Cart.Price;
                    _context.SaveChanges();
                }
                else
                {
                    Cart.Price = Cart.Quantity * Cart.Price;
                    Cart cartEntity = new Cart()
                    {
                        id = Cart.id,
                        Productid = Cart.Productid,
                        Quantity = Cart.Quantity,
                        Price = Cart.Price,
                        User = Cart.user,
                        Product = Cart.product,
                        Userid = Cart.Userid

                    };
                    await _repository.AddAsync(cartEntity);
                }

                return Ok(Cart);

            }
            return Ok("Email and Password is Incorrect");
        }



    }
}
