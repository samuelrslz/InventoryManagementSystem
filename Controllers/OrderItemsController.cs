using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public OrderItemsController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet]
        public IActionResult GetOrderItems()
        {
            return Ok(_context.OrderItems.ToList());
        }

        // GET: api/OrderItems/{id}
        [HttpGet("{id}")]
        public IActionResult GetOrderItem(int id)
        {
            var orderItem = _context.OrderItems
                .Where(oi => oi.Id == id)
                .Select(oi => new
                {
                    oi.Id,
                    oi.OrderId,
                    OrderDate = oi.Order.OrderDate,
                    oi.ProductId,
                    ProductName = oi.Product.Name,
                    oi.Quantity
                })
                .FirstOrDefault();

            if (orderItem == null)
            {
                return NotFound();
            }

            return Ok(orderItem);
        }

        // POST: api/OrderItems
        [HttpPost]
        public IActionResult CreateOrderItem(OrderItem orderItem)
        {
            var order = _context.Orders.Find(orderItem.OrderId);
            var product = _context.Products.Find(orderItem.ProductId);

            if (order == null || product == null)
            {
                return BadRequest("Order or Product not found.");
            }

            if (orderItem.Quantity > product.Quantity)
            {
                return BadRequest("Insufficient product quantity.");
            }

            product.Quantity -= orderItem.Quantity;

            _context.OrderItems.Add(orderItem);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.Id }, orderItem);
        }

        // PUT: api/OrderItems/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateOrderItem(int id, OrderItem updatedOrderItem)
        {
            var orderItem = _context.OrderItems.Find(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            orderItem.Quantity = updatedOrderItem.Quantity;
            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/OrderItems/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteOrderItem(int id)
        {
            var orderItem = _context.OrderItems.Find(id);
            if (orderItem == null)
            {
                return NotFound();
            }

            _context.OrderItems.Remove(orderItem);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
