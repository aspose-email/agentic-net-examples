using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Email address to validate
            string email = "example@example.com";

            // Validate the email address
            ValidationResult result;
            validator.Validate(email, out result);

            // Display the validation outcome
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
