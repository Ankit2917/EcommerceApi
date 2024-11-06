using EcommerceApi.Data;
using EcommerceApi.Data.IRepository;
using EcommerceApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/seller")]
    [ApiController]
    public class SellerController : ControllerBase
    {
        
        private readonly IRepository<Seller> _repository;
        public SellerController(  IRepository<Seller> repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task< IActionResult> AddSeller([FromBody]SellerDTO sellerDTO)
        {
               var alreadySeller = _repository.FirstOrDefault(x=>x.Email == sellerDTO.Email && x.Password == sellerDTO.Password);
            if(alreadySeller != null)
            {
                return Ok("Seller Already Registered Please Login");
            }
            if (sellerDTO != null)
            {
                Seller seller = new Seller()
                {
                    Email = sellerDTO.Email,
                    Password = sellerDTO.Password,
                    Name = sellerDTO.Name,
                    id = sellerDTO.id
                };
             await  _repository.AddAsync(seller);
              
                return StatusCode(201, seller);
            }
            return StatusCode(404);
        }

        [HttpPost]
        [Route("Login")]
       
        public async Task<IActionResult> Login([FromBody] Seller seller)
        {
            if (seller != null)
            {
                var Login = await _repository.FirstOrDefault(x=>x.Email == seller.Email && x.Password == seller.Password);
                
                return Ok(Login);
            }
            return Ok("Email and Password is Incorrect");
        }



    }
}
