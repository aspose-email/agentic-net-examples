using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string emailAddress = "test@example.com";

            // Create the validator
            EmailValidator emailValidator = new EmailValidator();

            // Perform validation
            ValidationResult validationResult;
            emailValidator.Validate(emailAddress, out validationResult);

            // Show user‑friendly result
            switch (validationResult.ReturnCode)
            {
                case ValidationResponseCode.ValidationSuccess:
                    Console.WriteLine($"The email address '{emailAddress}' is valid.");
                    break;
                case ValidationResponseCode.SyntaxValidationFailed:
                    Console.WriteLine($"The email address '{emailAddress}' has an invalid syntax.");
                    break;
                case ValidationResponseCode.DomainValidationFailed:
                    Console.WriteLine($"The domain part of the email address '{emailAddress}' could not be validated.");
                    break;
                case ValidationResponseCode.MailServerValidationError:
                    Console.WriteLine($"Mail server validation failed for '{emailAddress}'.");
                    break;
                default:
                    Console.WriteLine($"Validation returned an unknown result for '{emailAddress}'.");
                    break;
            }

            // Optional detailed message
            if (!string.IsNullOrEmpty(validationResult.Message))
            {
                Console.WriteLine($"Detail: {validationResult.Message}");
            }
        }
        catch (AsposeException ex)
        {
            Console.Error.WriteLine($"Aspose error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
