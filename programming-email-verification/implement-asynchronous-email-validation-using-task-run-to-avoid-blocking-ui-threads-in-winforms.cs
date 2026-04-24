using Aspose.Email;
using System;
using System.Threading.Tasks;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string emailAddress = "example@example.com";

            Task<ValidationResult> validationTask = ValidateEmailAsync(emailAddress);
            validationTask.Wait();

            ValidationResult result = validationTask.Result;

            Console.WriteLine($"Email: {emailAddress}");
            Console.WriteLine($"Return Code: {result.ReturnCode}");
            Console.WriteLine($"Message: {result.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    private static Task<ValidationResult> ValidateEmailAsync(string mailAddress)
    {
        return Task.Run(() =>
        {
            EmailValidator validator = new EmailValidator();
            ValidationResult validationResult;
            validator.Validate(mailAddress, out validationResult);
            return validationResult;
        });
    }
}
