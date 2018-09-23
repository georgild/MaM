using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels.Base {
    public class BaseEntityApiModel : BaseApiModel {

        public Guid CreatedBy { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public Guid ModifiedBy { get; set; }

        public string Title { get; set; }
    }
}
