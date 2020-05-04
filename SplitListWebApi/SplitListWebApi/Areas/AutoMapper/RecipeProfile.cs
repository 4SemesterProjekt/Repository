using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat.Recipe;
using AutoMapper;

namespace SplitListWebApi.Areas.AutoMapper
{
    public class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<RecipeDTO, RecipeModel>()
                .PreserveReferences()
                .ForMember(model => model.RecipeItems, opt => opt.Ignore());
            
            CreateMap<RecipeModel, RecipeDTO>()
                .PreserveReferences()
                .ForMember(dto => dto.Items, opt => opt.MapFrom(model => model.RecipeItems));
        }
    }
}
