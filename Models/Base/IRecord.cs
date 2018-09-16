using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Base {
    public interface IRecord : IBaseRecord {
        /// <summary>
        /// The Id of the user that created the record.
        /// </summary>
        Guid CreatedBy { get; set; }

        /// <summary>
        /// The date when the record was created.
        /// </summary>
        DateTime DateCreated { get; set; }

        /// <summary>
        /// The Id of the user that last modified the record.
        /// </summary>
        DateTime DateModified { get; set; }

        /// <summary>
        /// The date when the record was modified.
        /// </summary>
        Guid ModifiedBy { get; set; }

        /// <summary>
        /// Indicates if the entity is soft deleted.
        /// Defaults to false.
        /// </summary>
        bool Deleted { get; set; }
    }
}
