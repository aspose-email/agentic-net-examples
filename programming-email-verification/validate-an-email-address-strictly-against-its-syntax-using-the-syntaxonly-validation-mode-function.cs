using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Read the email address from the console.
            Console.Write("Enter email address to validate: ");
            string emailAddress = Console.ReadLine();

            // Create an EmailValidator instance.
            EmailValidator validator = new EmailValidator();

            // Validate the address using SyntaxOnly policy.
            ValidationResult result;
            validator.Validate(emailAddress, ValidationPolicy.SyntaxOnly, out result);

            // Display the validation outcome.
            Console.WriteLine("Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            // Write any unexpected errors to the error stream.
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
