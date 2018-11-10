using Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace CustomPolicyAuth {
    public class ContextPrincipal : ClaimsPrincipal {
        public ContextPrincipal() {
        }

        public ContextPrincipal(IPrincipal principal) : base(principal) {

            Claim userIdClaim = base.Claims.Where(x => x.Type.Equals(MaMClaimTypes.UserId)).FirstOrDefault();

            if (default(Claim) == userIdClaim) {
                throw new UnauthorizedAccessException(@"The principal is invalid: contains invalid claims!");
            }

            UserId = Guid.Parse(userIdClaim.Value);
        }

        public Guid UserId { get; set; }

        public Role Role { get; set; }

        // extend with more helper methods if needed
    }
}
