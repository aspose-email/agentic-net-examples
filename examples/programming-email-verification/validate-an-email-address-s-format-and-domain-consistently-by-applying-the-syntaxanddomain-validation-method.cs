using System;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Email address to validate
            string email = "example@example.com";

            // Create the validator instance
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Perform validation using SyntaxAndDomain policy
            Aspose.Email.Tools.Verifications.ValidationResult result;
            validator.Validate(email, Aspose.Email.Tools.Verifications.ValidationPolicy.SyntaxAndDomain, out result);

            // Output validation results
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            // Handle any unexpected errors gracefully
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}