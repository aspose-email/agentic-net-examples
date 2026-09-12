using Aspose.Email;
using Aspose.Email.Tools.Verifications;
using System;

namespace AsposeEmailValidationSample
{
    // Service interface for email validation
    public interface IEmailValidationService
    {
        ValidationResult Validate(string email);
    }

    // Implementation that uses Aspose.Email's EmailValidator
    public class EmailValidationService : IEmailValidationService
    {
        private readonly EmailValidator _validator;

        public EmailValidationService(EmailValidator validator)
        {
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        public ValidationResult Validate(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email address must be provided.", nameof(email));

            // Use the simple overload that applies the MailServer validation policy
            _validator.Validate(email, out ValidationResult result);
            return result;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Manually create the EmailValidator (acts as our DI container)
                EmailValidator validator = new EmailValidator();

                // Resolve the validation service with the validator injected
                IEmailValidationService validationService = new EmailValidationService(validator);

                // Example email address to validate
                string emailToValidate = "example@example.com";

                // Perform validation
                ValidationResult result = validationService.Validate(emailToValidate);

                // Output the result
                Console.WriteLine($"Email: {emailToValidate}");
                Console.WriteLine($"Return Code: {result.ReturnCode}");
                Console.WriteLine($"Message: {result.Message}");
                if (result.LastException != null)
                {
                    Console.WriteLine($"Error: {result.LastException.Message}");
                }
            }
            catch (Exception ex)
            {
                // Graceful error handling
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
