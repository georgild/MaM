using Models.Identity;
using Repository.Base;
using Repository.Contexts;
using RepositoryContracts.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Identity {
    public class OrganizationRepository : GenericRepository<Organization>, IOrganizationRepository {

        public OrganizationRepository(Context context)
             : base(context) { }
    }
}
