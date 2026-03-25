using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Sample email addresses to validate
            string[] emailAddresses = new string[] { "valid@example.com", "invalid-email", "user@nonexistentdomain.xyz" };

            // Initialize the email validator
            EmailValidator validator = new EmailValidator();

            foreach (string address in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(address, out result);

                Console.WriteLine("Email: " + address);
                Console.WriteLine("Return Code: " + result.ReturnCode);
                Console.WriteLine("Message: " + result.Message);
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