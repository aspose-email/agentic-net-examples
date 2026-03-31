using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            Console.WriteLine("Enter email address to validate:");
            string emailAddress = Console.ReadLine();

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Validate the email address using the default MailServer policy
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Report the validation outcome
            Console.WriteLine("Validation Return Code: " + result.ReturnCode);
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
