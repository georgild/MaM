using AutoMapper;
using BizModels.Identity;
using BusinessServicesContracts.Identity;
using BusinessServicesContracts.VFileSystem;
using BusinessServicesImpl.Base;
using CustomPolicyAuth;
using Models.Base;
using Models.Identity;
using Models.VFileSystem;
using RepositoryContracts.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServicesImpl.Identity {
    public class OrganizationServiceImpl : BaseEntityServiceImpl<Organization, OrganizationBizModel>, IOrganizationService {

        private readonly IOrganizationUnitOfWork organizationUnitOfWork;

        public OrganizationServiceImpl(
                IOrganizationUnitOfWork unitOfWork,
                IMapper mapper)
                : base(unitOfWork, mapper) 
        {

            organizationUnitOfWork = unitOfWork;
        }

        public override async Task<Guid> Create(ContextPrincipal principal, Organization organization) {

            organization.Id = Guid.NewGuid();
            organization.CreatedBy = principal.UserId;
            organization.ModifiedBy = principal.UserId;

            await base.InsertAncestors(organization, Entity.RootId);

            VFileSystemItem folder = new VFileSystemItem {
                Id = Guid.NewGuid(),
                Title = organization.Title,
                CreatedBy = principal.UserId,
                ModifiedBy = principal.UserId,
                DateCreated = DateTime.UtcNow,
                DateModified = DateTime.UtcNow,
                Deleted = false,
                Type = EVItemType.Folder
            };

            await base.InsertAncestors(folder, organization.Id);

            _unitOfWork.EntityResourceRepository.Insert(organization);
            organizationUnitOfWork.VFileSystemItemRepository.Insert(folder);

            int persistedObjects = await _unitOfWork.Save();
            if (persistedObjects <= 0) {
                throw new InvalidOperationException(@"No DB records have been persisted!");
            }

            return organization.Id;
        }
    }
}
