using System;

class Program
{
    static void Main()
    {
        try
        {
            // Email address to validate
            string email = "example@example.com";

            // Create the validator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Perform validation with SyntaxAndDomain policy
            Aspose.Email.Tools.Verifications.ValidationResult result;
            validator.Validate(email, Aspose.Email.Tools.Verifications.ValidationPolicy.SyntaxAndDomain, out result);

            // Output the validation result
            Console.WriteLine("Return Code: " + result.ReturnCode);
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