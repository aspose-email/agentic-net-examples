using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string email = "test@mailinator.com";

            // Predefined list of disposable domains
            string[] disposableDomains = new string[]
            {
                "mailinator.com",
                "trashmail.com",
                "10minutemail.com"
            };

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform validation using the default MailServer policy
            ValidationResult result;
            validator.Validate(email, out result);

            // Check the validation response code
            if (result.ReturnCode != ValidationResponseCode.ValidationSuccess)
            {
                Console.WriteLine($"Email validation failed: {result.Message}");
                return;
            }

            // Extract domain part of the email address
            int atIndex = email.LastIndexOf('@');
            if (atIndex < 0 || atIndex == email.Length - 1)
            {
                Console.WriteLine("Invalid email format.");
                return;
            }

            string domain = email.Substring(atIndex + 1).ToLowerInvariant();

            // Determine if the domain is disposable
            bool isDisposable = false;
            foreach (string d in disposableDomains)
            {
                if (domain == d)
                {
                    isDisposable = true;
                    break;
                }
            }

            if (isDisposable)
            {
                Console.WriteLine("The email address belongs to a disposable domain and is rejected.");
            }
            else
            {
                Console.WriteLine("The email address is valid and not disposable.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
