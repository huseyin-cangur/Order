
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderAPI.Dtos;

namespace OrderAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderContext _context;
        private readonly IMapper _mapper;

        public OrderService(IMapper mapper, OrderContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<CreateOrderDto> CreateOrder(CreateOrderDto orderDto)
        {
            var product = await _context.Products.FindAsync(orderDto.ProductId);
            if (product == null || product.Stock < orderDto.Quantity)
            {
                throw new Exception("Ürün stokta yok veya yetersiz.");
            }
            var order = _mapper.Map<Entity.Order>(orderDto);
            order.OrderDate = DateTime.UtcNow;
            await _context.Orders.AddAsync(order);

            product.Stock -= orderDto.Quantity;
            _context.Products.Update(product);

            if (await _context.SaveChangesAsync() > 0)
            {
                return orderDto;
            }
            throw new Exception("Sipariş oluşturulamadı.");


        }

        public async Task<OrderDto> GetById(Guid Id)
        {
            var order = await _context.Orders.Include(p => p.Product).Include(c => c.Customer)
                .Where(o => o.Id == Id)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    Name = o.Product.Name,
                    Price = o.Product.Price,
                    Stock = o.Product.Stock,
                    OrderDate = o.OrderDate,
                    Quantity = o.Quantity,
                    TotalPrice = o.TotalPrice
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                throw new Exception("Sipariş bulunamadı.");
            }

            return order;
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerId(Guid customerId)
        {

            var orders = await _context.Orders.Where(o => o.CustomerId == customerId).Include(p => p.Product).Include(c => c.Customer)
             .Select(o => new OrderDto
             {
                 Id = o.Id,
                 ProductId = o.ProductId,
                 Name = o.Product.Name,
                 Price = o.Product.Price,
                 Stock = o.Product.Stock,
                 OrderDate = o.OrderDate,
                 Quantity = o.Quantity,
                 TotalPrice = o.TotalPrice
             })
            .ToListAsync();

            if (orders == null || orders.Count == 0)
            {
                throw new Exception("Müşteriye ait sipariş bulunamadı.");
            }

            return orders;


        }

        public async Task<Guid> RemoveOrder(Guid Id)
        {
            var order = await _context.Orders.FindAsync(Id);
            if (order == null)
            {
                throw new Exception("Sipariş bulunamadı.");
            }

            var product = await _context.Products.FindAsync(order.ProductId);
            if (product != null)
            {
                product.Stock += order.Quantity;
                _context.Products.Update(product);
            }

            _context.Orders.Remove(order);

            if (await _context.SaveChangesAsync() > 0)
            {
                return Id;
            }

            throw new Exception("Sipariş silinemedi.");
        }
    }
}