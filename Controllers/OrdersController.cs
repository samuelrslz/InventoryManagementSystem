using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public OrdersController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public IActionResult GetOrders()
        {
            return Ok(_context.Orders.ToList());
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders
                .Where(o => o.Id == id)
                .Select(o => new
                {
                    o.Id,
                    o.OrderDate,
                    o.CustomerId,
                    Customer = o.Customer.Name,
                    Items = o.OrderItems.Select(oi => new
                    {
                        oi.ProductId,
                        oi.Product.Name,
                        oi.Quantity
                    })
                })
                .FirstOrDefault();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Orders
        [HttpPost]
        public IActionResult CreateOrder(OrderCreateDto orderDto)
        {
            if (orderDto == null || orderDto.OrderItems == null || !orderDto.OrderItems.Any())
            {
                return BadRequest("Invalid order data.");
            }

            var order = new Order
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = orderDto.OrderDate,
                OrderItems = orderDto.OrderItems.Select(item => new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // PUT: api/Orders/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateOrder(int id, Order updatedOrder)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.OrderDate = updatedOrder.OrderDate;
            order.CustomerId = updatedOrder.CustomerId;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Orders/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
