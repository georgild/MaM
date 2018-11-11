using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BizModels.Tasks {
    public class MetadataTaskBizModel : TaskBizModel {

        [Required]
        public string Scope { get; set; }
    }
}
