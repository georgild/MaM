using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Identity {
    public class User : IdentityUser<Guid>, IActor {
        public User() {
            this.Id = new Guid();
        }

        /// <summary>
        /// The Address of the user.
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// Can be activated/deactivated.
        /// </summary>
        [Required]
        public bool Active { get; set; }

        /// <summary>
        /// Whether or not is deleted by admin.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// Navigation property to the refresh token.
        /// </summary>
        public virtual RefreshToken RefreshToken { get; set; }
    }
}
