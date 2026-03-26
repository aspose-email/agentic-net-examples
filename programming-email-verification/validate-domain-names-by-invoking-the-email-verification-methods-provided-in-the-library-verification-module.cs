using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Prepare a variable to receive the validation result
            ValidationResult result;

            // Validate the email address using the default MailServer validation policy
            validator.Validate("example@example.com", out result);

            // Output the validation outcome
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}