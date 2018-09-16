using Microsoft.AspNetCore.Authorization;
using Models.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Models.Identity;
using Models.Base;
using System.Linq;

namespace CustomPolicyAuth {
    public class AuthHandler : AuthorizationHandler<CustomAuthRequirement> {
        private const string ResourceVariableName = nameof(CustomAuthorize.Resource);
        private const string OperationVariableName = nameof(CustomAuthorize.Operation);

        /// <summary>
        /// The purpose of this handler is to make the authorization check before any controller action, possibly
        /// using authorization filter in the pipeline and put before the controller's action declaration.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="requirement"></param>
        /// <returns></returns>
        protected override async Task HandleRequirementAsync(
                                                       AuthorizationHandlerContext context,
                                                       CustomAuthRequirement requirement) {
            try {

                //NOTE: Downcasting is required to be able to use "MethodInfo" method.
                var authorizationFilterContext = (Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext)context.Resource;
                var controllerActionDescriptor = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)authorizationFilterContext.ActionDescriptor);
                MethodInfo methodInfo = controllerActionDescriptor.MethodInfo;

                EResourceType resource = (EResourceType)this.GetCustomAttributeProperty(methodInfo, ResourceVariableName);
                EOperation operation = (EOperation)this.GetCustomAttributeProperty(methodInfo, OperationVariableName);

                if (operation == EOperation.Invalid || resource == EResourceType.Invalid) {
                    throw new UnauthorizedAccessException("Could not find permission or resource.");
                }

                if (authorizationFilterContext.HttpContext == null) {
                    throw new UnauthorizedAccessException("HttpContex is null.");
                }

                // The "entityId" holds the Id of the resource for which we are doing the current operation.
                // It is extracted from the request's endpoint path.
                // Basically all entities are attached in the tree to a parent. But if no entity(or parent entity) Id is sent we are
                // passing the Root folder Id. This is valid for objects like Users which are logically attached to the Root.
                object entityId;
                bool gotEntityId = authorizationFilterContext.RouteData.Values.TryGetValue(AuthConstants.IdNameInActionAttributeRouting, out entityId);

                Guid entityIdAsGuid = Entity.RootId;
                if (gotEntityId) {
                    Guid.TryParse((string)entityId, out entityIdAsGuid);
                }

                var user = authorizationFilterContext.HttpContext.User;
                ContextPrincipal principal = new ContextPrincipal(user);

                Role userRole = new Role(); // await permissionsBusinessRepo.GetCurrentRole(principal.UserId, entityIdAsGuid);
                principal.Role = userRole ?? throw new UnauthorizedAccessException("Could not find role.");

                bool canPerformRequiredOperation = true; // permissionsBusinessRepo.CheckPermissionsByActorId(userRole.Id, entityIdAsGuid, resource, operation);

                if (!canPerformRequiredOperation) {
                    throw new UnauthorizedAccessException("User does not have this permission.");
                }

                context.Succeed(requirement);
                return;
            }
            catch (UnauthorizedAccessException) {
                context.Fail();
            }
            catch (Exception) {
                context.Fail();
            }
        }

        private object GetCustomAttributeProperty(MethodInfo methodInfo, string variableName) {
            object attributeValue = methodInfo
                .CustomAttributes
                .Where(attribute => attribute.AttributeType == typeof(CustomAuthorize))
                .FirstOrDefault()
                .NamedArguments
                .Where(namedArgument => namedArgument.MemberName.Equals(variableName))
                .Select(arg => arg.TypedValue.Value)
                .FirstOrDefault();

            if (attributeValue == null) {
                throw new UnauthorizedAccessException();
            }

            return attributeValue;
        }
    }
}
