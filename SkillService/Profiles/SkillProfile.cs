using AutoMapper;
using SkillService.Dtos;
using SkillService.Models;

namespace SkillService.Profiles
{
    public class SkillProfile : Profile
    {
        public SkillProfile()
        {
            // Source -> Target
            CreateMap<Certificate, CertificateReadDto>();
            CreateMap<SkillCreateDto, Skill>();
            CreateMap<Skill, SkillReadDto>();
            CreateMap<CertificatePublishedDto, Certificate>().ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
            CreateMap<CertificateService.GrpcCertificateModel, Certificate>()
                .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.CertificateId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Skills, opt => opt.Ignore());
        }
    }
}
