using System;
using System.Collections.Generic;
using System.Text;

namespace ApiModels.Base {
    public abstract class BaseApiModel {
        public Guid Id { get; set; }

        public string Title { get; set; }
    }
}
