using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Security {
    [Flags]
    public enum EOperation : long {
        Invalid = 0,
        Read = 1,
        List = 2,
        Create = 4,
        Update = 8,
        Delete = 16,
        ListPermissions = 32,
        ModifyPermissions = 64
    }
}
