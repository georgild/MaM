using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BizModels.Tasks {
    public class TranscoderTaskBizModel: TaskBizModel {

        [Required]
        public string Format { get; set; }
    }
}
