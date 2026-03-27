using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Tools.Verifications;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Server configuration
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Validate the email address (includes mail server validation)
                EmailValidator validator = new EmailValidator();
                ValidationResult validationResult;
                validator.Validate(username, out validationResult);
                Console.WriteLine($"Email validation return code: {validationResult.ReturnCode}");
                if (!string.IsNullOrEmpty(validationResult.Message))
                {
                    Console.WriteLine($"Validation message: {validationResult.Message}");
                }

                // Validate SMTP credentials
                using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
                {
                    try
                    {
                        bool credentialsValid = client.ValidateCredentials();
                        Console.WriteLine($"SMTP credentials valid: {credentialsValid}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP validation failed: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
