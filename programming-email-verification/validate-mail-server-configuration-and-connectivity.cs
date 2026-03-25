using System;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of EmailValidator
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            // Perform validation using the MailServer validation policy
            Aspose.Email.Tools.Verifications.ValidationResult result;
            validator.Validate("test@example.com", out result);

            // Output the validation result
            Console.WriteLine($"Return code: {result.ReturnCode}");
            Console.WriteLine($"Message: {result.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}