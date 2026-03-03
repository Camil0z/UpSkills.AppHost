using AutoMapper;
using UpSkills.Infrastructure.Entidades;
using UpSkills.Models.DTO;

namespace UpSkills.Business.Libs;

public class AutoMapperConfig : Profile
{
    public AutoMapperConfig()
    {
        CreateMap<Pais, PaisDTO>().ReverseMap();
        
        CreateMap<Rol, RolDTO>().ReverseMap();
        
        CreateMap<Usuario, AuthDTO>().ReverseMap();

        CreateMap<Usuario, UsuarioDTO>()
                .ForMember(dto => dto.Rol, opt => opt.MapFrom(ent => ent.Rol.Id))
                .ForMember(dto => dto.Pais, opt => opt.MapFrom(ent => ent.Pais.Id))
            .ReverseMap();
                //.ForMember(ent => ent.Rol, opt => opt.MapFrom(dto => new Rol { Id = dto.Rol }))
                //.ForMember(ent => ent.Pais, opt => opt.MapFrom(dto => new Pais { Id = dto.Pais }));

        CreateMap<Usuario, GetUsuarioDTO>()
                .ForMember(dto => dto.Rol, opt => opt.MapFrom(ent => ent.Rol.Id))
                .ForMember(dto => dto.Pais, opt => opt.MapFrom(ent => ent.Pais == null ? (long?)null : ent.Pais.Id))
            .ReverseMap()
                .ForMember(ent => ent.Rol, opt => opt.MapFrom(dto => new Rol { Id = dto.Rol }))
                .ForMember(ent => ent.Pais, opt => opt.MapFrom(dto => (dto.Pais == null) ? null : new Pais { Id = (long)dto.Pais! }));

        CreateMap<Usuario, CreateUsuarioDTO>()
                .ForMember(dto => dto.Rol, opt => opt.MapFrom(ent => ent.Rol.Id))
                .ForMember(dto => dto.Pais, opt => opt.MapFrom(ent => ent.Pais == null ? (long?)null : ent.Pais.Id))
            .ReverseMap()
                .ForMember(ent => ent.Rol, opt => opt.MapFrom(dto => new Rol { Id = dto.Rol }))
                .ForMember(ent => ent.Pais, opt => opt.MapFrom(dto => (dto.Pais == null) ? null : new Pais { Id = (long)dto.Pais! }));
    }
}
