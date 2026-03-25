using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            EmailValidator validator = new EmailValidator();

            string[] emailAddresses = new string[]
            {
                "valid.user@example.com",
                "invalid-email",
                "user@nonexistentdomain.xyz"
            };

            foreach (string address in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(address, out result);

                Console.WriteLine("Email: " + address);
                Console.WriteLine("Return Code: " + result.ReturnCode);
                Console.WriteLine("Message: " + result.Message);
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}