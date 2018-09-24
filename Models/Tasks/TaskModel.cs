using Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models.Tasks {
    public class TaskModel: Entity {

        public DateTime EndedAt { get; set; }

        [Required]
        public double Progress { get; set; }

        [Required]
        public DateTime StartedAt { get; set; }

        [Required]
        public ETaskType Type { get; set; }
    }
}
