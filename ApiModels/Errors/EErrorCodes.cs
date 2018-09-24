using System;
using System.Collections.Generic;
using System.Text;

namespace BizModels.Errors {
    public enum EErrorCodes {
        InvalidModelStateErrorCode = 1,
        GeneralUnknownErrorCode = 2,
        EmailSendingErrorCode = 3,
        ForbiddenAccessErrorCode = 4,

        GeneralInsertErrorCode = 1000,
        GeneralDeleteErrorCode = 1001,
        GeneralUpdateErrorCode = 1002,
        GeneralGetErrorCode = 1003,

        UserSignInErrorCode = 2000,
        UserTokenGenerationErrorCode = 2001,
        UserConfirmErrorCode = 2002,
        UserUnconfirmErrorCode = 2003,
        UserActivateErrorCode = 2004,
        UserDeactivateErrorCode = 2005,
        UserPasswordsDontMatchErrorCode = 2006,
        UserPasswordResetErrorCode = 2007,
        UserPasswordUpdateErrorCode = 2008
    }
}
