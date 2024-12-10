namespace InventoryManagementSystem.Models
{
    public class OrderCreateDto
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public List<OrderItemCreateDto> OrderItems { get; set; } = new();
    }

    public class OrderItemCreateDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}