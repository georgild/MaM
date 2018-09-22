using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels.Identity {
    public class UserApiModel {

        public Guid Id { get; set; }

        /// <summary>
        /// The Address of the user.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Can be activated/deactivated.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Whether or not is deleted by admin.
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Whether or not the user has confirmed
        /// </summary>
        public bool EmailConfirmed { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string UserName { get; set; }
    }
}
