using AgencySystemDotNet.ViewModels.Admin;
using AgencySystemDotNet.ViewModels.Editor;
using AgencySystemDotNet.ViewModels.PostQuestion;

using AutoMapper;

using PressAgencyApp.Models;
using PressAgencyApp.ViewModels.Customer;
using PressAgencyApp.ViewModels.Editor;
using PressAgencyApp.ViewModels.Post;
using PressAgencyApp.ViewModels.PostQuestion;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AgencySystemDotNet
{
    public class AutoMapperConfig
    {
        
public static MapperConfiguration Configure()
        {
            MapperConfiguration mapperConfiguration = new MapperConfiguration(cfg => {
               cfg. CreateMap<Post, PostViewModelR>()
               .ForMember(x => x.CategoryName, opt => opt.MapFrom(x => x.Category == null ? "" : x.Category.Title))
               .ForMember(x => x.EditorName, opt => opt.MapFrom(x => x.Editor == null ? "" : x.Editor.FirstName + " " + x.Editor.LastName))
               .ForMember(x => x.Questions, opt => opt.MapFrom(x => x.Questions))
               ;
                cfg. CreateMap<Post, PostViewModelU>()
                .ForMember(x=>x.Photo,opt=>opt.Ignore())
             
               ;
                cfg.CreateMap<PostQuestion, PostQuestionViewModelU>();
                cfg.CreateMap<PostQuestionViewModelR, PostQuestion>();
                cfg. CreateMap<PostQuestion, PostQuestionViewModelR>()
                    .ForMember(x => x.CustomerName, opt => opt.MapFrom(x => x.Customer == null ? "" : x.Customer.FirstName + " " + x.Customer.LastName))
                    ;
                cfg.CreateMap<User, AdminViewModelU>()
                .ForMember(x => x.Photo, opt => opt.Ignore());
                cfg.CreateMap<User, EditorViewModelU>()
                .ForMember(x => x.Photo, opt => opt.Ignore());
                cfg. CreateMap<User, EditorViewModelR>()
                .ForMember(x => x.Posts, opt => opt.MapFrom(x => x.Posts));
                cfg. CreateMap<User, AdminViewModelR>();
                cfg. CreateMap<User, CustomerViewModelR>();
                cfg. CreateMap<User, CustomerViewModelU>()
                .ForMember(x => x.Photo, opt => opt.Ignore());

            });

            return mapperConfiguration;
        }
    }
}