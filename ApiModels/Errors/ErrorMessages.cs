using System;
using System.Collections.Generic;
using System.Text;

namespace BizModels.Errors {
    public class ErrorMessages {
        private readonly Dictionary<EErrorCodes, string> errorCodes = new Dictionary<EErrorCodes, string>();

        private const string InvalidModelStateError = "The request contains invalid properties.";
        private const string GeneralUnknownError = "Unknown error occurred.";
        private const string EmailSendingError = "Could not send e-mail.";
        private const string ForbiddenAccessError = "Unauthorized.";

        private const string GeneralInsertError = "Could not insert record.";
        private const string GeneralDeleteError = "Could not delete record.";
        private const string GeneralUpdateError = "Could not update record.";
        private const string GeneralGetError = "Could not find record(s).";

        private const string UserSignInError = "User can't sign-in.";
        private const string UserTokenGenerationError = "Could not generate token.";
        private const string UserConfirmError = "Could not confirm e-mail.";
        private const string UserUnconfirmError = "Could not unconfirm user.";
        private const string UserActivateError = "Could not activate user.";
        private const string UserDeactivateError = "Could not deactivate user.";
        private const string UserPasswordsDontMatchError = "Passwords don't match.";
        private const string UserPasswordResetError = "Could not reset password.";
        private const string UserPasswordUpdateError = "Could not update password.;";

        public ErrorMessages() {
            this.AddGeneralAppErros();
            this.AddGeneralCrudErrors();
            this.AddUserErrors();
        }

        public string GetErrorMessage(EErrorCodes errorCode) {
            string error = this.errorCodes[errorCode];
            if (string.IsNullOrEmpty(error)) {
                throw new System.Exception("Could not find error");
            }

            return error;
        }

        private void AddUserErrors() {
            errorCodes.Add(EErrorCodes.UserSignInErrorCode, UserSignInError);
            errorCodes.Add(EErrorCodes.UserTokenGenerationErrorCode, UserTokenGenerationError);
            errorCodes.Add(EErrorCodes.UserConfirmErrorCode, UserConfirmError);
            errorCodes.Add(EErrorCodes.UserUnconfirmErrorCode, UserUnconfirmError);
            errorCodes.Add(EErrorCodes.UserActivateErrorCode, UserActivateError);
            errorCodes.Add(EErrorCodes.UserDeactivateErrorCode, UserDeactivateError);
            errorCodes.Add(EErrorCodes.UserPasswordsDontMatchErrorCode, UserPasswordsDontMatchError);
            errorCodes.Add(EErrorCodes.UserPasswordResetErrorCode, UserPasswordResetError);
            errorCodes.Add(EErrorCodes.UserPasswordUpdateErrorCode, UserPasswordUpdateError);
        }

        private void AddGeneralCrudErrors() {
            errorCodes.Add(EErrorCodes.GeneralInsertErrorCode, GeneralInsertError);
            errorCodes.Add(EErrorCodes.GeneralDeleteErrorCode, GeneralDeleteError);
            errorCodes.Add(EErrorCodes.GeneralUpdateErrorCode, GeneralUpdateError);
            errorCodes.Add(EErrorCodes.GeneralGetErrorCode, GeneralGetError);
        }

        private void AddGeneralAppErros() {
            errorCodes.Add(EErrorCodes.InvalidModelStateErrorCode, InvalidModelStateError);
            errorCodes.Add(EErrorCodes.GeneralUnknownErrorCode, GeneralUnknownError);
            errorCodes.Add(EErrorCodes.EmailSendingErrorCode, EmailSendingError);
            errorCodes.Add(EErrorCodes.ForbiddenAccessErrorCode, ForbiddenAccessError);
        }
    }
}
