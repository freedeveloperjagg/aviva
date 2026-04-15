using AvivaApi.Bo;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using System.Net;

namespace AvivaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentBo xbo) : ControllerBase
    {
        readonly IPaymentBo bo = xbo;

        [HttpPost]
        public async Task<IActionResult> PostOrder([FromBody] OrderPago orderPago)
        {
            try
            {
                OrderResponse? order = await bo.CreatePaymentAsync(orderPago);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);                
            }
        }

    }
}
