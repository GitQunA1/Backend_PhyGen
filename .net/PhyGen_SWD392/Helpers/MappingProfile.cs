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
                .ForMember(dest => dest.SubjectName, opt => opt.MapFrom(src => src.Subject != null ? src.Subject.Name : null))
                .ReverseMap()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level.ToString()));

            CreateMap<TopicRequest, Topic>()
                .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.LevelEnum.ToString()));

            CreateMap<Subject, SubjectDTO>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.GradeEnum))
                .ReverseMap()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.Grade.ToString()));

            CreateMap<SubjectRequest, Subject>()
                .ForMember(dest => dest.Grade, opt => opt.MapFrom(src => src.GradeEnum.ToString()));

            CreateMap<ExamMatrix, ExamMatrixDTO>()
                .ForMember(dest => dest.Examtype, opt => opt.MapFrom(src => src.ExamTypeEnum))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusEnum))
                .ReverseMap()
                .ForMember(dest => dest.Examtype, opt => opt.MapFrom(src => src.Examtype.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<ExamMatrixRequest, ExamMatrix>()
                .ForMember(dest => dest.Examtype, opt => opt.MapFrom(src => src.ExamTypeEnum.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.StatusEnum.ToString()));

            CreateMap<MatrixSection, MatrixSectionDTO>();

            CreateMap<MatrixSectionRequest, MatrixSection>();

            CreateMap<MatrixDetail, MatrixDetailDTO>()
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.DifficultyLevel))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.SectionName, opt => opt.MapFrom(src => src.MatrixSection != null ? src.MatrixSection.SectionName : null))
                .ForMember(dest => dest.TopicName, opt => opt.MapFrom(src => src.Topic != null ? src.Topic.Name : null));

            CreateMap<MatrixDetailDTO, MatrixDetail>();

            CreateMap<MatrixDetailRequest, MatrixDetail>()
                .ForMember(dest => dest.DifficultyLevel, opt => opt.MapFrom(src => src.LevelEnum.ToString()))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.TypeEnum.ToString()));

            CreateMap<Question, QuestionDto>();


        }
    }
}
