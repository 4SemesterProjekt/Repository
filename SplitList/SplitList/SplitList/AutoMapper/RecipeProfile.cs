using System;
using System.Collections.Generic;
using System.Text;
using ApiFormat.Recipe;
using AutoMapper;
using SplitList.Models;

namespace SplitList.AutoMapper
{
    class RecipeProfile : Profile
    {
        public RecipeProfile()
        {
            CreateMap<RecipeDTO, Recipe>()
                .PreserveReferences()
                .ForMember(r => r.Id, opt => opt.MapFrom(rDto => rDto.ModelId))
                .ForMember(r => r.Ingredients, opt => opt.MapFrom(rDto => rDto.Items));

            CreateMap<Recipe, RecipeDTO>()
                .PreserveReferences()
                .ForMember(rDto => rDto.ModelId, opt => opt.MapFrom(r => r.Id))
                .ForMember(rDto => rDto.Items, opt => opt.MapFrom(r => r.Ingredients));
        }
    }
}
