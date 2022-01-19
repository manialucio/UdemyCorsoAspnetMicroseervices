using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages;
using EventBus.Messages.Events;

namespace Basket.API.Mappings
{
    public class OrderingProfile : Profile
    {
        public OrderingProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent> ().ReverseMap();
         }

    }
}
