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
            string email = "example@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Validate the email address
            ValidationResult result;
            validator.Validate(email, out result);

            // Display validation results
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            // Write any unexpected errors to the error console
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}