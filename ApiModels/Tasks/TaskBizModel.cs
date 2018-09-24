using BizModels.Base;
using Models.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BizModels.Tasks {
    public class TaskBizModel : BaseEntityBizModel {

        public DateTime EndedAt { get; set; }

        [Required]
        public double Progress { get; set; }

        [Required]
        public DateTime StartedAt { get; set; }

        [Required]
        public ETaskState State { get; set; }

        [Required]
        public ETaskType Type { get; set; }
    }
}
