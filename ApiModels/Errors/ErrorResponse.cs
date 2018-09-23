using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApiModels.Errors {
    public class ErrorResponse {

        private readonly ErrorMessages messages = new ErrorMessages();

        public ErrorResponse() {
            this.Errors = new List<Error>();
            this.TimestampInSeconds = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public List<Error> Errors { get; set; }

        public double TimestampInSeconds { get; set; }

        /// <summary>
        /// Adds error to our Errors list by error code.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public ErrorResponse AddError(EErrorCodes code) {
            var errorMessage = messages.GetErrorMessage(code);
            this.Errors.Add(new Error((int)code, errorMessage));

            return this;
        }

        public ErrorResponse AddError(EErrorCodes code, string additionalDetails) {
            var errorMessage = messages.GetErrorMessage(code);
            var error = new Error((int)code, errorMessage, additionalDetails);

            this.Errors.Add(error);

            return this;
        }

        /// <summary>
        /// Adds ModelStateDictionary's errors to our Errors list.
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public ErrorResponse AddModelStateErrors(ModelStateDictionary modelState) {
            if (modelState == null || !modelState.Values.Any()) {
                throw new ArgumentException("ModelStateDictionary is empty or null.");
            }

            IEnumerable<ModelError> modelStateErrors = modelState.Values.SelectMany(v => v.Errors);
            foreach (var modelStateError in modelStateErrors) {
                this.AddError(EErrorCodes.InvalidModelStateErrorCode, modelStateError.ErrorMessage);
            }

            return this;
        }
    }
}
