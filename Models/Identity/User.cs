using Microsoft.AspNetCore.Identity;
using Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Identity {
    public class User : Entity, IActor {
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

        [Required]
        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Navigation property to the refresh token.
        /// </summary>
        public virtual RefreshToken RefreshToken { get; set; }
    }
}
