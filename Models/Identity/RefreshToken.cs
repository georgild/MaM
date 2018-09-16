using Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Identity {
    public class RefreshToken : IBaseRecord {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The refresh token generated from the authorization server. 
        /// </summary>
        public string RefreshTokenValue { get; set; }

        /// <summary>
        /// This should be user identificator (UUID, email, etc)
        /// </summary>
        public string Subject { get; set; }

        public DateTime IssuedUtc { get; set; }

        public DateTime ExpiresUtc { get; set; }

        /// <summary>
        /// References the user that claims this token.
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// Navigation property to the user.
        /// </summary>
        public virtual User User { get; set; }
    }
}
