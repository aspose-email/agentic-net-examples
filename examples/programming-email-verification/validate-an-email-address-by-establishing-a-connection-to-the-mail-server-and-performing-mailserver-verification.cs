using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create an instance of EmailValidator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Email address to validate
            string emailAddress = "user@example.com";

            // Perform validation using the MailServer validation policy
            Aspose.Email.Tools.Verifications.ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Output validation results
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