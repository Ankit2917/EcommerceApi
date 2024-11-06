using EcommerceApi.Data.IRepository;
using EcommerceApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<User> _repository;
        public UserController(IRepository<User> repository)
        {

            _repository = repository;
        }

        [HttpPost]

        public async Task<IActionResult> AddUser([FromBody] UserDTO userDTO)
        {
            var alreadyUser = await _repository.FirstOrDefault(x => x.Email == userDTO.email && x.Password == userDTO.password);
            if (alreadyUser != null)
            {
                return Ok("User Already Registered Please Login");
            }
            if (userDTO != null)
            {
                User user = new User()
                {
                    Email = userDTO.email,
                    Name = userDTO.name,
                    id = userDTO.id,
                    Password = userDTO.password
                };

                await _repository.AddAsync(user);
                return StatusCode(201, user);
            }
            return StatusCode(404);
        }


        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] User seller)
        {
            if (seller != null)
            {
                var Login = await _repository.FirstOrDefault(x => x.Email == seller.Email && x.Password == seller.Password);

                return Ok(Login);
            }
            return Ok("Email and Password is Incorrect");
        }

       
       
    }
}
