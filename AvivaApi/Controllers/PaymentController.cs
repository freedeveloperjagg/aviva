using AvivaApi.Bo;
using AvivaLibrary.Models;
using AvivaLibrary.Models.Requests;
using AvivaLibrary.Models.Responses;
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
                List<OrderResponse> response = await bo.GetOrdersAsync();
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
        /// <param name="provider">Provider Name</param>
        /// <returns>One OrderResponse</returns>
        [HttpGet("Order")]
        public async Task<IActionResult> GetOrder([FromQuery]string id, [FromQuery] string provider)
        {
            try
            {
                OrderResponse? response = await bo.GetOrderAsync(id,provider);
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
 
        /// <summary>
        /// Cancel the order by Id an provider Name
        /// </summary>
        /// <param name="request">
        /// Contain the Order Id and the Provider Name
        /// </param>
        /// <returns>Ok if ok Status 500 if error.</returns>
        [HttpPut("cancel")]
        public async Task<IActionResult> CancelOrder([FromBody] ChangeOrderRequest request)
        {
            try
            {
                await bo.CancelOrderAsync(request);
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
        /// <param name="request"></param>
        /// <response>OK</response>
        [HttpPut("pay")]
        public async Task<IActionResult> PayOrder([FromBody] ChangeOrderRequest request)
        {
            try
            {
                await bo.PayOrderAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
