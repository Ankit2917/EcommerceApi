using EcommerceApi.Data;
using EcommerceApi.Data.IRepository;
using EcommerceApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceApi.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        private readonly IRepository<Seller> _seller;
        private readonly IRepository<User> _userRepo;

        public TokenController(IConfiguration config, ApplicationDbContext context, IRepository<Seller> seller, IRepository<User>  userrepo)
        {
            _configuration = config;
            _seller = seller;
            _userRepo = userrepo;
        }

        [HttpPost]
        [Route("SellerToken")]

        public async Task<IActionResult> Post(SellerDTO seller)
        {
            if (seller.Email != null && seller.Password !=null )
            {
                var sellerdetails = await _seller.FirstOrDefault(x=>x.Email == seller.Email && x.Password == seller.Password);
                if(sellerdetails != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("SellerId",sellerdetails.id.ToString()),
                        new Claim("SellerName",sellerdetails.Name.ToString()),
                        new Claim("SellerEmail",sellerdetails.Email.ToString()),
                        new Claim("Role","Seller"),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                       _configuration["Jwt:Issuer"],
                       _configuration["Jwt:Audience"],
                       claims,
                       expires: DateTime.UtcNow.AddMinutes(10),
                       signingCredentials: signIn);

                    return Ok(new {token = new JwtSecurityTokenHandler().WriteToken(token)});
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Invalid credentials");
            }

        }


        [HttpPost]
        [Route("UserToken")]

        public async Task<IActionResult> PostUser(UserDTO user)
        {
            if (user.email != null && user.password != null)
            {
                var Userdetails = await _userRepo.FirstOrDefault(x => x.Email == user.email && x.Password == user.password);
                if (Userdetails != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                        new Claim("UserId",Userdetails.id.ToString()),
                        new Claim("UserrName",Userdetails.Name.ToString()),
                        new Claim("UserEmail",Userdetails.Email.ToString()),
                        new Claim("Role","User"),
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                       _configuration["Jwt:Issuer"],
                       _configuration["Jwt:Audience"],
                       claims,
                       expires: DateTime.UtcNow.AddMinutes(10),
                       signingCredentials: signIn);

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest("Invalid credentials");
            }

        }


    }
}
