﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace Models.AspNetDummyParents {
    public class CustomIdentityUserToken : IdentityUserToken<Guid> {
    }
}
