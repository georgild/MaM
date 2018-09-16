using Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Security {
    public class ACEntry : IBaseRecord {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// The User or Role of the ACEntry.
        /// </summary>
        [Required]
        public Guid ActorId { get; set; }

        /// <summary>
        /// The permission of the ACEntry
        /// </summary>
        [Required]
        public EOperation Operation { get; set; }

        /// <summary>
        /// The Entity, that owns the path.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// The type of entry - grant or deny.
        /// </summary>
        [Required]
        public EACEntryType Type { get; set; }

        /// <summary>
        /// Defines the resource type the ACEntry is applied to.
        /// </summary>
        public EResourceType ResourceType { get; set; }

        /// <summary>
        /// The Entity, affected by the ACEntry.
        /// </summary>
        [NotMapped]
        public Entity Entity { get; set; }
    }
}
