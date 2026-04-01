using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            Console.Write("Enter email address to validate: ");
            string email = Console.ReadLine();

            EmailValidator validator = new EmailValidator();

            ValidationResult result;
            validator.Validate(email, ValidationPolicy.SyntaxAndDomain, out result);

            Console.WriteLine("Validation Return Code: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
