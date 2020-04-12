using AutoMapper;
using SEPMTool.Models;
using SEPMTool.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SEPMTool.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectViewModel>();
            CreateMap<ProjectRequirement, RequirementViewModel>();
            CreateMap<RequirementTask, RequirementTaskViewModel>();
            CreateMap<SubTask, SubTaskViewModel>();
            CreateMap<ProjectUser, ProjectUserViewModel>();
            CreateMap<TaskUser, TaskUserViewModel>();
        }
    }
}
