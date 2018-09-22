﻿using ApiModels.Identity;
using AutoMapper;
using Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels {
    public class MappingProfile : Profile {
        public MappingProfile() {

            CreateMap<UserApiModel, User>();
            CreateMap<User, UserApiModel>();

            CreateMap<RoleApiModel, Role>();
            CreateMap<Role, RoleApiModel>();
        }
    }
}
