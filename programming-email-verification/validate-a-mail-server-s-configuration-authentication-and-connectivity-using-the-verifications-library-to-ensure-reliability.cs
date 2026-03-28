using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Tools.Verifications;

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

            Console.WriteLine("Email validation ReturnCode: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
            if (validationResult.LastException != null)
            {
                Console.WriteLine("Exception: " + validationResult.LastException.Message);
            }

            // Validate server connectivity and credentials
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    bool credentialsValid = client.ValidateCredentials();
                    Console.WriteLine("Credentials valid: " + credentialsValid);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error validating credentials: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
