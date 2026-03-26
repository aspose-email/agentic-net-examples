using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // List of email addresses to validate
            string[] emailAddresses = new string[]
            {
                "test@example.com",
                "invalid-email",
                "user@nonexistentdomain.tld"
            };

            // Validate each email address
            foreach (string address in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(address, out result);

                Console.WriteLine("Email: " + address);
                Console.WriteLine("Return Code: " + result.ReturnCode);
                if (!string.IsNullOrEmpty(result.Message))
                {
                    Console.WriteLine("Message: " + result.Message);
                }
                if (result.LastException != null)
                {
                    Console.WriteLine("Exception: " + result.LastException.Message);
                }
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}