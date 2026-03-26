using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Tools.Verifications;

namespace AsposeEmailVerificationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // SMTP server connection parameters
                string smtpHost = "smtp.example.com";
                int smtpPort = 587;
                string smtpUsername = "user@example.com";
                string smtpPassword = "password";

                // Validate SMTP credentials
                try
                {
                    using (Aspose.Email.Clients.Smtp.SmtpClient smtpClient = new Aspose.Email.Clients.Smtp.SmtpClient())
                    {
                        smtpClient.Host = smtpHost;
                        smtpClient.Port = smtpPort;
                        smtpClient.Username = smtpUsername;
                        smtpClient.Password = smtpPassword;
                        smtpClient.SecurityOptions = Aspose.Email.Clients.SecurityOptions.Auto;

                        bool credentialsValid = smtpClient.ValidateCredentials();
                        Console.WriteLine($"SMTP credentials valid: {credentialsValid}");
                    }
                }
                catch (Exception smtpEx)
                {
                    Console.Error.WriteLine($"SMTP validation error: {smtpEx.Message}");
                }

                // Email address validation using Aspose.Email verification API
                string emailToValidate = "test@example.com";
                EmailValidator validator = new EmailValidator();
                ValidationResult validationResult;
                validator.Validate(emailToValidate, out validationResult);

                Console.WriteLine($"Email validation return code: {validationResult.ReturnCode}");
                if (!string.IsNullOrEmpty(validationResult.Message))
                {
                    Console.WriteLine($"Message: {validationResult.Message}");
                }
                if (validationResult.LastException != null)
                {
                    Console.Error.WriteLine($"Exception: {validationResult.LastException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}