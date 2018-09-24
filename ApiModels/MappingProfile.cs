using BizModels.Identity;
using BizModels.VFileSystem;
using AutoMapper;
using Models.Identity;
using Models.VFileSystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels {
    public class MappingProfile : Profile {
        public MappingProfile() {

            CreateMap<UserBizModel, User>();
            CreateMap<User, UserBizModel>();

            CreateMap<RoleBizModel, Role>();
            CreateMap<Role, RoleBizModel>();
        }
    }
}
