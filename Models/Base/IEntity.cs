using Models.Security;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Base {
    public interface IEntity : IRecord {
        /// <summary>
        /// The Title of the entity (can be the display name shown for Folder or Document in the UI).
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The ACEntries specifying the Access Control for this Entity.
        /// Not mapped, because there is no actual foreign key.
        /// TODO [bzh]  Remove this property if not removed already.
        ///             We should have Collection of Associations instead here:
        ///             Association: User-with-Role-to-Entity.
        ///             We support Associations to Folders only.
        ///             Furthermore on business layer we allow Associations to ROOT, Practice, and Engagement only.
        /// </summary>
        ICollection<ACEntry> ACEntries { get; set; }
    }
}
