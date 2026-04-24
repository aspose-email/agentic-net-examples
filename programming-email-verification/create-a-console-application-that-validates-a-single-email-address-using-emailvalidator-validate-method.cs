using Aspose.Email;
using System;
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
            EmailValidator validator = new EmailValidator();

            // Perform validation
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            // Output the result
            Console.WriteLine("Validation Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Validation Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
