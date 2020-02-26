using AutoMapper;
using Services.Repository.Models;
using Services.Repository.Models.DatabaseModels;
using Services.Repository.Models.DataTransferObjects;
using System.Collections.Generic;

namespace Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<List<ProductDto>, Product>();
            CreateMap<ReleaseNote, ReleaseNoteDto>();
            CreateMap<ReleaseNoteDto, ReleaseNote>();
            CreateMap<WorkItem, WorkItemDto>();
            CreateMap<WorkItemDto, WorkItem>();
        }
    }
}
