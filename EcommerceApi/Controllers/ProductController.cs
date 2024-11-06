using EcommerceApi.Data;
using EcommerceApi.Data.IRepository;
using EcommerceApi.Data.Utility;
using EcommerceApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace EcommerceApi.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;



        private readonly IWebHostEnvironment _env;
        private readonly ApplicationDbContext _context;
        public ProductController(IRepository<Product> repository, IWebHostEnvironment env, ApplicationDbContext context)
        {
            _productRepository = repository;
            _env = env;
            _context = context;
        }
        [HttpPost]

        public async Task<IActionResult> AddProduct([FromForm] ProductDTO product)
        {
            if (product != null)
            {

                Product productOg = new Product();
                Random random = new Random();
                var number = random.Next(1, 100000);
                var uploadsFolderPath = Path.Combine(_env.WebRootPath, "Image");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                if (product.imageurl != null)
                {
                    var filePath = Path.Combine(uploadsFolderPath, number + product.imageurl.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.imageurl.CopyToAsync(stream);
                    }
                    var relativePath = Path.Combine("/Image", number + product.imageurl.FileName).Replace("\\", "/");
                    productOg.imageurl = relativePath;
                }




                productOg.Name = product.name;
                productOg.Colour = product.colour;
                productOg.Description = product.description;
                productOg.Price = product.price;
                await _productRepository.AddAsync(productOg);
                return Ok(product);
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductDTO product)
        {
            var uploadsFolderPath = Path.Combine(_env.WebRootPath);
            var productInDB = await _productRepository.FirstOrDefault(x => x.id == product.id);

            if (product.imageurl != null)
            {
                if (System.IO.File.Exists(uploadsFolderPath + productInDB.imageurl))
                {
                    System.IO.File.Delete(uploadsFolderPath + productInDB.imageurl);
                }
                Random random = new Random();
                var number = random.Next(1, 100000);
                uploadsFolderPath = Path.Combine(_env.WebRootPath, "Image");

                var filePath = Path.Combine(uploadsFolderPath, number + product.imageurl.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.imageurl.CopyToAsync(stream);
                }

                var relativePath = Path.Combine("/Image", number + product.imageurl.FileName).Replace("\\", "/");
                productInDB.imageurl = relativePath;
            }

            productInDB.Price = product.price;
            productInDB.Description = product.description;
            productInDB.Colour = product.colour;
            productInDB.id = product.id;
            productInDB.Name = product.name;
            _context.SaveChanges();

            return Ok(new { message = "Product Updated Successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var list = await _productRepository.GetAllAsync();
            foreach (var item in list)
            {
                item.imageurl = "https://localhost:7241/" + item.imageurl;
            }

            return Ok(list);
        }
        [HttpGet]
        [Route("ProductById")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var Product = await _productRepository.FirstOrDefault(x => x.id == id);
            Product.imageurl = "https://localhost:7241/" + Product.imageurl;
            return Ok(Product);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var uploadsFolderPath = Path.Combine(_env.WebRootPath);
            var productInDB = await _productRepository.FirstOrDefault(x => x.id == id);

            if (System.IO.File.Exists(uploadsFolderPath + productInDB.imageurl))
            {
                System.IO.File.Delete(uploadsFolderPath + productInDB.imageurl);
            }


            await _productRepository.DeleteAsync(id);
            return Ok(new { message = "Deleted Successfully" });
        }

        [HttpGet]
        [Route("Search")]
        public IActionResult Search(string? query)
        {
            var list = _context.products.Where(x => x.Name.Contains(query)).ToList();
            return Ok(list);
        }


    }
}
