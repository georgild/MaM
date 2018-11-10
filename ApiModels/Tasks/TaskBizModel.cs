using BizModels.Base;
using Models.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BizModels.Tasks {
    public class TaskBizModel : BaseEntityBizModel {

        public DateTime EndedAt { get; set; }

        public double Progress { get; set; }

        public DateTime StartedAt { get; set; }

        public ETaskState State { get; set; }

        [Required]
        public ETaskType Type { get; set; }
    }
}
