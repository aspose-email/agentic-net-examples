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
            string email = "test@example.com";

            // Create the validator
            EmailValidator validator = new EmailValidator();

            // Perform validation
            ValidationResult result;
            validator.Validate(email, out result);

            // Display validation outcome
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
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