using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Enter email address to validate: ");
            string emailAddress = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(emailAddress))
            {
                Console.Error.WriteLine("No email address provided.");
                return;
            }

            EmailValidator validator = new EmailValidator();

            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            Console.WriteLine("Validation Return Code: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
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
