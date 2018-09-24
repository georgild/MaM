using System;
using System.Collections.Generic;
using System.Text;

namespace BizModels.Errors {
    public class Error {
        public Error(int code, string description) {
            this.Code = code;
            this.Description = description;
        }

        public Error(int code, string description, string additionalDetails) : this(code, description) {
            this.AdditionalDetails = additionalDetails;
        }

        public int Code { get; set; }

        public string Description { get; set; }

        public string AdditionalDetails { get; set; }
    }
}
