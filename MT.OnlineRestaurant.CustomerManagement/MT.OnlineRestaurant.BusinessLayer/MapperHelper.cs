using AutoMapper;
using MT.OnlineRestaurant.BusinessEntities;
using MT.OnlineRestaurant.DataLayer.Context;
using System.Collections.Generic;
using System.Linq;

namespace MT.OnlineRestaurant.BusinessLayer
{
    public class MapperHelper:Profile
    {
        public MapperHelper()
        {
            CreateMap<CustomerDetails, TblCustomer>()
                .ForMember(dest => dest.Email, source => source.MapFrom(src => src.UserName))
                            .ForMember(dest => dest.Password, opt => opt.Ignore());


            CreateMap<TblCustomer, CustomerDetails>()
            .ForMember(dest => dest.UserName, source => source.MapFrom(src => src.Email))
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
