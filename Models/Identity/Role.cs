using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Identity {
    public class Role : IdentityRole<Guid>, IActor {
        /// <summary>
        /// The Id of the Audit Practice the Role belongs to. Can be null for Root level roles.
        /// </summary>
        public Guid? PracticeId { get; set; }
    }
}
