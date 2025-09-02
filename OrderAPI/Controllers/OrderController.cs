

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Dtos;
using OrderAPI.Services;

namespace OrderAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public IActionResult CreateOrder(CreateOrderDto createOrderDto)
        {

            return Ok(_orderService.CreateOrder(createOrderDto));
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid Id)
        {
            return Ok(await _orderService.GetById(Id));
        }

        [HttpGet]
        public async Task<IActionResult> GetOrdersByCustomerId(Guid customerId)
        {
            return Ok(await _orderService.GetOrdersByCustomerId(customerId));
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveOrder(Guid Id)
        {
            return Ok(await _orderService.RemoveOrder(Id));
        }



    }
}