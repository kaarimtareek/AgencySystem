using AutoMapper;

using PressAgencyApp.Models;
using PressAgencyApp.ViewModels.Editor;
using PressAgencyApp.ViewModels.Post;
using PressAgencyApp.ViewModels.PostQuestion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PressAgencyApp
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Post, PostViewModelR>()
                .ForMember(x=>x.CategoryName,opt=>opt.MapFrom(x=>x.Category==null? "":x.Category.Title))
                .ForMember(x=>x.EditorName,opt=>opt.MapFrom(x=>x.Editor==null? "":x.Editor.FirstName + " "+ x.Editor.LastName))
                .ForMember(x=>x.Questions,opt=>opt.MapFrom(x=>x.Questions))
                ;
            CreateMap<PostQuestion, PostQuestionViewModelR>()
                .ForMember(x=>x.CustomerName,opt=>opt.MapFrom(x=>x.Customer==null? "":x.Customer.FirstName + " "+ x.Customer.LastName ))
                ;
            CreateMap<User, EditorViewModelR>()
                .ForMember(x=>x.Posts,opt=>opt.MapFrom(x=>x.Posts))
                ;

        }
    }
}
