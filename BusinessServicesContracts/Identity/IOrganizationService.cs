﻿using BizModels.Identity;
using BusinessServicesContracts.Base;
using Models.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessServicesContracts.Identity {
    public interface IOrganizationService : IBaseEntityService<Organization, OrganizationBizModel> {
    }
}
