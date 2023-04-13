using AutoMapper;
using Zuzka.Data.Entities;
using Zuzka.Services.DTO;

namespace Zuzka.Api.Profiles
{
    public class DocumentProfile : Profile
    {
        public DocumentProfile()
        {
            CreateMap<Document, DocumentDto>()
            .ForMember(d => d.Author, opt => opt.MapFrom(src => src.Data != null ? src.Data.Author : string.Empty))
            .ForMember(d => d.PublishedYear, opt => opt.MapFrom(src => src.Data != null ? src.Data.PublishedYear : default))
            .ForMember(d => d.IsBestseller, opt => opt.MapFrom(src => src.Data != null ? src.Data.IsBestseller : false))
            .ForMember(d => d.Rating, opt => opt.MapFrom(src => src.Data != null ? src.Data.Rating : default))
            .ForMember(d => d.Title, opt => opt.MapFrom(src => src.Data != null ? src.Data.Title : string.Empty));

            CreateMap<DocumentRequestDto, Document>()
               .ForPath(d => d.Data.Author, opt => opt.MapFrom(src => src.Author))
               .ForPath(d => d.Data.PublishedYear, opt => opt.MapFrom(src => src.PublishedYear))
               .ForPath(d => d.Data.IsBestseller, opt => opt.MapFrom(src => src.IsBestseller))
               .ForPath(d => d.Data.Rating, opt => opt.MapFrom(src => src.Rating))
               .ForPath(d => d.Data.Title, opt => opt.MapFrom(src => src.Title))
               .ForPath(dest => dest.Created, opt => opt.Ignore());
        }
    }
}
