using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to be validated
            string emailAddress = "example@example.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Validate the email address using SyntaxAndDomain policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation result
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
