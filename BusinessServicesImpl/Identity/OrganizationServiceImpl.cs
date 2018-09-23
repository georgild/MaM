﻿using BusinessServicesContracts.Identity;
using BusinessServicesImpl.Base;
using Models.Identity;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesImpl.Identity {
    public class OrganizationServiceImpl : BaseEntityServiceImpl<Organization>, IOrganizationService {

        public OrganizationServiceImpl(
                IBaseEntityUnitOfWork<Organization> unitOfWork)
            : base(unitOfWork) {
        }
    }
}