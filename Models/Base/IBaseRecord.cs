using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Models.Base {
    public interface IBaseRecord {
        /// <summary>
        /// Will be used as a Primary Key in the implementing classes.
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        Guid Id { get; set; }
    }
}
