using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        // Author: Aspose.Email example - Email address syntax validation
        string[] emailAddresses = new string[]
        {
            "user@example.com",
            "invalid-email",
            "test@domain",
            "john.doe@sub.example.co.uk"
        };

        EmailValidator validator = new EmailValidator();

        foreach (string address in emailAddresses)
        {
            ValidationResult result;
            // Validate using syntax‑only policy
            validator.Validate(address, ValidationPolicy.SyntaxOnly, out result);

            bool isValid = result.ReturnCode == ValidationResponseCode.ValidationSuccess;
            Console.WriteLine($"{address} => {(isValid ? "Valid" : "Invalid")}");
        }
    }
}
