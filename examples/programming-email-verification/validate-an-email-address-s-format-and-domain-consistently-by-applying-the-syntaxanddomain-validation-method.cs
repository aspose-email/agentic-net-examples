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

            // Create the validator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Perform validation with SyntaxAndDomain policy
            Aspose.Email.Tools.Verifications.ValidationResult result;
            validator.Validate(emailAddress, Aspose.Email.Tools.Verifications.ValidationPolicy.SyntaxAndDomain, out result);

            // Output the validation result
            Console.WriteLine("Return code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}