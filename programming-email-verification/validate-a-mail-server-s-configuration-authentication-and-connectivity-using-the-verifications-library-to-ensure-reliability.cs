using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Tools.Verifications;

namespace EmailServerValidationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder configuration – skip real network calls if placeholders are detected
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";
                string emailToValidate = "user@example.com";

                if (host.Contains("example.com") || emailToValidate.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder configuration detected. Skipping external validation.");
                    return;
                }

                // Validate server credentials
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = host;
                    client.Port = port;
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto;

                    try
                    {
                        bool credentialsValid = client.ValidateCredentials();
                        Console.WriteLine($"Credentials validation result: {credentialsValid}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during credentials validation: {ex.Message}");
                        return;
                    }
                }

                // Validate email address (includes mail server validation)
                EmailValidator emailValidator = new EmailValidator();
                ValidationResult validationResult;
                emailValidator.Validate(emailToValidate, out validationResult);
                Console.WriteLine($"Email validation return code: {validationResult.ReturnCode}");

                if (validationResult.ReturnCode != ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine($"Validation message: {validationResult.Message}");
                    if (validationResult.LastException != null)
                    {
                        Console.WriteLine($"Exception: {validationResult.LastException.Message}");
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
