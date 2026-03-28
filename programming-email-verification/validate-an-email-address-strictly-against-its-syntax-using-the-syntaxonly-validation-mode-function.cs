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
            string emailAddress = "example@domain.com";

            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Validate the email address using SyntaxOnly policy
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxOnly, out validationResult);

            // Display validation outcome
            Console.WriteLine("Return code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
