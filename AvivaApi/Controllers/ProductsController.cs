using AvivaApi.Bo;
using AvivaLibrary.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AvivaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductsBo xbo) : ControllerBase
    {
        private readonly IProductsBo bo = xbo;

        [HttpGet]
        public async Task<IActionResult> GetProductsAsync()
        {
            try
            {
                List<Product> products = await bo.GetProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving products: {ex.Message}");
            }
        }
    }
}
