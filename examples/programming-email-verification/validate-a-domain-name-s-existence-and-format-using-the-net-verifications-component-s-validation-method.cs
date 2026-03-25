using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emailAddress;
            if (args.Length > 0)
            {
                emailAddress = args[0];
            }
            else
            {
                Console.Write("Enter email address to validate: ");
                emailAddress = Console.ReadLine();
            }

            // Create an EmailValidator instance
            EmailValidator validator = new EmailValidator();

            // Validate the email address using syntax and domain checks
            ValidationResult validationResult;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxAndDomain, out validationResult);

            // Output the validation result
            Console.WriteLine("Return Code: " + validationResult.ReturnCode);
            if (!string.IsNullOrEmpty(validationResult.Message))
            {
                Console.WriteLine("Message: " + validationResult.Message);
            }
            if (validationResult.LastException != null)
            {
                Console.WriteLine("Exception: " + validationResult.LastException.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}