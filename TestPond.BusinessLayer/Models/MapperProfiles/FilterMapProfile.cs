using System;
using AutoMapper;

namespace TestPond.BusinessLayer.Models.MapperProfiles
{
    public class FilterMapProfile : Profile
    {
        public FilterMapProfile() 
        {
            CreateMap<Models.NUnit.Filter, Models.Filter>();
        }
    }
}
