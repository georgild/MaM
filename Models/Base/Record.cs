using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Base {
    public class Record : IRecord {
        public Guid Id { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid ModifiedBy { get; set; }
        public bool Deleted { get; set; }
    }
}
