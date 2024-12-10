using InventoryManagementSystem.Data;
using InventoryManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly InventoryDbContext _context;

        public CustomersController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers
        [HttpGet]
        public IActionResult GetCustomers()
        {
            return Ok(_context.Customers.ToList());
        }

        // GET: api/Customers/{id}
        [HttpGet("{id}")]
        public IActionResult GetCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public IActionResult CreateCustomer(CustomerCreateDto customerDto)
        {
            if (customerDto == null)
            {
                return BadRequest("Customer data is invalid.");
            }

            var customer = new Customer
            {
                Name = customerDto.Name,
                Email = customerDto.Email,
                Phone = customerDto.Phone
            };

            _context.Customers.Add(customer);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }

        // PUT: api/Customers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, Customer updatedCustomer)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Customers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return NoContent();
        }
    }
}