using AutoMapper;
using OZQ_cart.Models;
using OZQ_cart.DTOs;

namespace OZQ_cart.AutoMapper
{
    public class MyAutoMapperProfile : Profile
    {
        public MyAutoMapperProfile()
        {
            // Define your mappings here using the CreateMap method
            CreateMap<Customer, CustomerToReturnDto>();
            CreateMap<CustomerToInsertDto, Customer>();
            CreateMap<Feedback, FeedbackToReturnDto>();
            CreateMap<FeedbackToInsertDto, Feedback>();
            CreateMap<Item, ItemToReturnDto>();
            CreateMap<ItemToInsertDto, Item>();
            CreateMap<Order, OrderToReturnDto>();
            CreateMap<OrderToInsertDto, Order>();
            CreateMap<OrderItem, OrderItemToReturnDto>();
            CreateMap<OrderItemToInsertDto, OrderItem>();
        }
    }
}
