using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        // Author note: Simple console example demonstrating strict syntax validation of an email address.
        string email = "example@example.com";

        EmailValidator validator = new EmailValidator();

        ValidationResult result;
        validator.Validate(email, ValidationPolicy.SyntaxOnly, out result);

        if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
        {
            Console.WriteLine($"Email '{email}' is syntactically valid.");
        }
        else
        {
            Console.WriteLine($"Email '{email}' is invalid. Reason: {result.Message}");
        }
    }
}
