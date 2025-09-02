

namespace OrderAPI.Dtos
{
    public class CreateOrderDto
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }

    }
}