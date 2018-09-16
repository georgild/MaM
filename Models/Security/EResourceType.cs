using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Security {
    public enum EResourceType {
        Invalid = 0,
        Practice = 1,
        Folder = 2,
        Document = 3,
        User = 4,
        Client = 5,
        Engagement = 6,
        Event = 7,

        Organization = 101
    }
}
