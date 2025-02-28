using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using AutoMapper;
using ExamWCF.Entities;

namespace ExamWCF.DTOs
{
    public static class Mapping
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<ModelMapping>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }
    public class ModelMapping : Profile
    {
        public ModelMapping()
        {
            CreateMap<Faculty, FacultyDTO>().ReverseMap();
            CreateMap<Major, MajorDTO>().ReverseMap();
            CreateMap<Alumni, AlumniDTO>().ReverseMap();
            CreateMap<JobHistory, JobHistoryDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<District, DistrictDTO>().ReverseMap();
            CreateMap<AlumniImage, AlumniImageDTO>().ReverseMap();
            CreateMap<Hobby, HobbyDTO>().ReverseMap();
            CreateMap<JobPosting, JobPostingDTO>().ReverseMap();
            CreateMap<AttachmentType, AttachmentTypeDTO>().ReverseMap();
            CreateMap<EmploymentType, EmploymentTypeDTO>().ReverseMap();
            CreateMap<JobAttachment, JobAttachmentDTO>().ReverseMap();
            CreateMap<JobAttachmentType, JobAttachmentTypeDTO>().ReverseMap();
            CreateMap<JobSkill, JobSkillDTO>().ReverseMap();
            CreateMap<Skill, SkillsDTO>().ReverseMap();
            CreateMap<JobCandidate, JobCandidateDTO>().ReverseMap();
            CreateMap<Event, EventDTO>().ReverseMap();
            CreateMap<PhotoAlbum, PhotoAlbumDTO>().ReverseMap();
            CreateMap<Photo, PhotoDTO>().ReverseMap();
        }
    }
}