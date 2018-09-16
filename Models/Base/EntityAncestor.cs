using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Base {
    public class EntityAncestor : IBaseRecord {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Id of an Entity up the hierarchy.
        /// </summary>
        // Includes the Entity itself. Null for the root.
        public Guid? AncestorId { get; set; }

        /// <summary>
        /// The Entity, that owns the path.
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// The rank of the Ancestor, starting with 1 from the root.
        /// </summary>
        [Required]
        public int Rank { get; set; }

        /// <summary>
        /// Navigation property for the Entity this is an ancestor of. Not a mistake.
        /// Not mapped, because there is no actual foreign key.
        /// </summary>
        [NotMapped]
        public Entity Entity { get; set; }
    }
}
