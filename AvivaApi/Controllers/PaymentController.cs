using AvivaApi.Bo;
using AvivaLibrary.Models;
using Microsoft.AspNetCore.Mvc;


namespace AvivaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentBo xbo) : ControllerBase
    {
        readonly IPaymentBo bo = xbo;

        /// <summary>
        /// Get all the orders in all providers
        /// This return all the orders existent in all associated providers.
        /// </summary>
        /// <returns>A list of OrderResponse</returns>
        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrders()
        {
            try
            {
                List<OrderCreated> response = await bo.GetOrdersAsync();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get One Orders give nthe ID and the provider of pay name.
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>One OrderResponse</returns>
        [HttpGet("Order")]
        public async Task<IActionResult> GetOrderAsync([FromQuery] int id)
        {
            try
            {
                OrderCreated? response = await bo.GetOrderByIdAsync(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Create a Order in the provider selected internally
        /// </summary>
        /// <param name="orderPago"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderPago orderPago)
        {
            try
            {
                OrderCreated? order = await bo.CreatePaymentAsync(orderPago);
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Cancel the order by Id an provider Name
        /// </summary>
        /// <param name="id">
        /// Contain the Order Id and the Provider Name
        /// </param>
        /// <returns>Ok if ok Status 500 if error.</returns>
        [HttpPut("cancel")]
        public async Task<IActionResult> CancelOrder([FromQuery] int id)
        {
            try
            {
                await bo.CancelOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Pay the orders by ID
        /// </summary>
        /// <param name="id"></param>
        /// <response>OK</response>
        [HttpPut("pay")]
        public async Task<IActionResult> PayOrder([FromQuery] int id)
        {
            try
            {
                await bo.PayOrderAsync(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
