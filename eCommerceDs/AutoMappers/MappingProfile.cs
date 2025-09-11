using AutoMapper;
using eCommerceDs.DTOs;
using eCommerceDs.Models;

namespace eCommerceDs.AutoMappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Group, GroupDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageGroup))
                .ReverseMap()
                .ForMember(dest => dest.ImageGroup, opt => opt.MapFrom(src => src.ImageUrl));
                
            CreateMap<Group, GroupInsertDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageGroup))
                .ReverseMap()
                .ForMember(dest => dest.ImageGroup, opt => opt.MapFrom(src => src.ImageUrl));
                
            CreateMap<Group, GroupItemDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageGroup))
                .ReverseMap()
                .ForMember(dest => dest.ImageGroup, opt => opt.MapFrom(src => src.ImageUrl));
                
            CreateMap<Group, GroupUpdateDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageGroup))
                .ReverseMap()
                .ForMember(dest => dest.ImageGroup, opt => opt.MapFrom(src => src.ImageUrl));
                
            CreateMap<Group, GroupRecordsDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageGroup))
                .ReverseMap()
                .ForMember(dest => dest.ImageGroup, opt => opt.MapFrom(src => src.ImageUrl));
            CreateMap<GroupDTO, GroupItemDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreItemDTO>().ReverseMap();
            CreateMap<MusicGenreDTO, MusicGenreItemDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreInsertDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreUpdateDTO>().ReverseMap();
            CreateMap<MusicGenre, MusicGenreTotalGroupsDTO>().ReverseMap();
            CreateMap<Record, RecordDTO>().ReverseMap();
            CreateMap<Record, RecordInsertDTO>().ReverseMap();
            CreateMap<Record, RecordUpdateDTO>().ReverseMap();
            CreateMap<Record, RecordItemDTO>().ReverseMap();
            CreateMap<Record, RecordItemExtDTO>().ReverseMap();
            CreateMap<RecordDTO, RecordItemExtDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, UserInsertDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<CartDetail, CartDetailDTO>();
            CreateMap<CartDetailDTO, CartDetail>()
                .ForMember(dest => dest.Record, opt => opt.Ignore());
            CreateMap<Cart, CartDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();
        }

    }
}
