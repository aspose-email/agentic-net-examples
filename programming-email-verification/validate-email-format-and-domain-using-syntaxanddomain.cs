using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate
            string emailAddress = "example@example.com";

            // Create an instance of EmailValidator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Perform validation using SyntaxAndDomain policy
            Aspose.Email.Tools.Verifications.ValidationResult validationResult;
            validator.Validate(emailAddress, Aspose.Email.Tools.Verifications.ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation result
            Console.WriteLine("Return code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors gracefully
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}