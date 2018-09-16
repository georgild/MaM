using Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Base {
    public class BaseRecord : IBaseRecord {

        public Guid Id { get; set; }
    }
}
