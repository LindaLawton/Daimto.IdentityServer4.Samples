using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace SampleIdentityServer
{
    public static class ServiceAccountRequiredProperty
    {
        public const string Subject = "subject";
    }

    public class ServiceAccountGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _validator;

        public ServiceAccountGrantValidator(ITokenValidator validator)
        {
            _validator = validator;
        }

        public string GrantType => "serviceaccount";

        async Task IExtensionGrantValidator.ValidateAsync(ExtensionGrantValidationContext context)
        {
            // The clinet must be setup with the subject property.
            var prop = context.Request.Client.Properties.FirstOrDefault(a => a.Key.Equals(ServiceAccountRequiredProperty.Subject));
            if (prop.Key == null)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidClient);
                return;
            }

            // Grab the subject from the request.
            var subject = context.Request.Raw.Get(ServiceAccountRequiredProperty.Subject);
            if (prop.Value != subject)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }

            if (!long.TryParse(subject, out var subjectid))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest);
                return;
            }

            context.Result = new GrantValidationResult(subject, GrantType);
        }
    }
}
