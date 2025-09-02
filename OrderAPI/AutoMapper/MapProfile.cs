

using AutoMapper;
using OrderAPI.Dtos;
using OrderAPI.Entity;

namespace OrderAPI.AutoMapper
{
    public class MapProfile : Profile
    {
        protected MapProfile()
        {
            CreateMap<CreateOrderDto, Order>();
            CreateMap<Order, CreateOrderDto>();
        }
    }
}