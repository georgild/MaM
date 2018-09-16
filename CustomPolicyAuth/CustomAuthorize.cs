using Microsoft.AspNetCore.Authorization;
using Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace CustomPolicyAuth {
    public class CustomAuthorize : AuthorizeAttribute {
        public CustomAuthorize() {
            this.Policy = "CustomPolicy";
        }

        public EResourceType Resource { get; set; }

        public EOperation Operation { get; set; }
    }
}
