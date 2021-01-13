using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using backend.Controllers;
using backend.DAL;
using backend.Models;
using Microsoft.AspNetCore.Identity;

namespace backend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Question, QuestionDto>();
            CreateMap<QuestionDto, Question>();

            CreateMap<QuizDto, Quiz>();
            CreateMap<Quiz, QuizDto>();

            CreateMap<IdentityUser, Credentials>();
            CreateMap<Credentials, IdentityUser>()
                .ForMember(dest=>dest.UserName,
                    src=> src.MapFrom(x=>x.Email));
        }
    }
}
