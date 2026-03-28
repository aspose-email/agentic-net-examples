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

            // Email address to validate
            string emailAddress = "example@domain.com";

            // Perform validation using the default MailServer policy
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Output the validation result
            Console.WriteLine("Email: " + emailAddress);
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
