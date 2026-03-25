using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string emailAddress = "example@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Perform validation using the default MailServer policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Display validation outcome
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
            if (validationResult.LastException != null)
            {
                Console.WriteLine("Exception: " + validationResult.LastException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}