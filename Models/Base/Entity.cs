using Models.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Base {
   public class Entity : IEntity {
        /// <summary>
        /// System ROOT folder ID constant.
        /// </summary>
        public static readonly Guid RootId;

        static Entity() {
            RootId = Guid.Parse("a5a5a5a5-b6b6-c7c7-d9d9-a12a12a12a12");
        }

        /// <summary>
        /// The Primary key of the table.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// The date when the record was created.
        /// </summary>
        [Required]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// The Id of the user that last modified the record.
        /// </summary>
        [Required]
        public DateTime DateModified { get; set; }

        /// <summary>
        /// The date when the record was modified.
        /// </summary>
        [Required]
        public Guid ModifiedBy { get; set; }

        /// <summary>
        /// Indicates if the entity is soft deleted.
        /// Defaults to false.
        /// </summary>
        [Required]
        public bool Deleted { get; set; }

        /// <summary>
        /// The Title of the entity (can be the display name shown for Folder or Document in the UI).
        /// </summary>
        [Required]
        public string Title { get; set; }

        // Navigation properties
        /// <summary>
        /// The ACEntries specifying the Access Control for this Entity.
        /// Not mapped, because there is no actual foreign key.
        /// TODO [bzh]  Remove this property if not removed already.
        ///             We should have Collection of Associations instead here:
        ///             Association: User-with-Role-to-Entity.
        ///             We support Associations to Folders only.
        ///             Furthermore on business layer we allow Associations to ROOT, Practice, and Engagement only.
        /// </summary>
        [NotMapped]
        public ICollection<ACEntry> ACEntries { get; set; }
    }
}
