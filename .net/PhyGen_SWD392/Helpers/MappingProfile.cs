namespace PhyGen_SWD392.Helpers
{
    using AutoMapper;
    using PhyGen_SWD392.Models.DTOs;
    using PhyGen_SWD392.Models.Entities;
    using PhyGen_SWD392.Models.Enums;
    using PhyGen_SWD392.Models.Requests;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Topic, TopicDTO>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LevelEnum))
                .ReverseMap()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()));

            CreateMap<TopicRequest, Topic>()
                //.ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LevelEnum.ToString()));

            CreateMap<Subject, SubjectDTO>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.GradeEnum))
                .ReverseMap()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.ToString()));

            CreateMap<SubjectRequest, Subject>()
                //.ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.GradeEnum.ToString()));

            CreateMap<ExamMatrix, ExamMatrixDTO>()
                .ForMember(dest => dest.Examtype, opt => opt.MapFrom(src => src.ExamTypeEnum))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusEnum))
                .ReverseMap()
                .ForMember(dest => dest.Examtype, opt => opt.MapFrom(src => src.Examtype.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ExamMatrixRequest, ExamMatrix>()
                //.ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Examtype, opt => opt.MapFrom(src => src.ExamTypeEnum.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusEnum.ToString()));

            CreateMap<MatrixSection, MatrixSectionDTO>();
            //.ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<MatrixSectionRequest, MatrixSection>();
                //.ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
