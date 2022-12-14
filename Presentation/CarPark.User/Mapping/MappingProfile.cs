using AutoMapper;
using CarPark.Entities.Concrete;
using CarPark.Models.ViewModels.Personels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarPark.User.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Personel, PersonelProfileInfo>().ReverseMap();
        }
    }
}
