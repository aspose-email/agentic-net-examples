using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Email address to validate
                string emailToValidate = "test@example.com";

                // Establish a connection to the SMTP server (example settings)
                using (Aspose.Email.Clients.Smtp.SmtpClient smtpClient = new Aspose.Email.Clients.Smtp.SmtpClient())
                {
                    smtpClient.Host = "smtp.example.com";
                    smtpClient.Port = 587;
                    smtpClient.Username = "username";
                    smtpClient.Password = "password";
                    smtpClient.SecurityOptions = Aspose.Email.Clients.SecurityOptions.Auto;

                    // Optional: validate SMTP credentials
                    bool credentialsValid = smtpClient.ValidateCredentials();
                    Console.WriteLine("SMTP credentials valid: " + credentialsValid);
                }

                // Validate the email address using MailServer validation policy
                Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();
                Aspose.Email.Tools.Verifications.ValidationResult result;
                validator.Validate(emailToValidate, out result);

                // Output validation results using verified members
                Console.WriteLine("Validation Return Code: " + result.ReturnCode);
                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine("Message: " + result.Message);
                }
                if (result.LastException != null)
                {
                    Console.WriteLine("Exception: " + result.LastException.Message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}