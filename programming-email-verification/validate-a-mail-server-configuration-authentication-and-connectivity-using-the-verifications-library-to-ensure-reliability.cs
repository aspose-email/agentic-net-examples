using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of the EmailValidator
            EmailValidator validator = new EmailValidator();

            // Validate an email address using the default MailServer validation policy
            ValidationResult result;
            validator.Validate("test@example.com", out result);

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