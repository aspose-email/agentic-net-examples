using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create an EmailValidator instance and set the timeout to 5 seconds (5000 ms)
            EmailValidator validator = new EmailValidator();
            validator.Timeout = 5000;

            // Email address to validate
            string emailAddress = "example@example.com";

            // Perform validation
            ValidationResult result;
            validator.Validate(emailAddress, out result);

            // Check validation result using ReturnCode
            if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
            {
                Console.WriteLine("Email address is valid.");
            }
            else
            {
                Console.WriteLine($"Email validation failed: {result.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
