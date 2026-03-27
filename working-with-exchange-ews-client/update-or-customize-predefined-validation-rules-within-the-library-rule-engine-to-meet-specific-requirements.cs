using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string emailAddress = "example@example.com";

            EmailValidator validator = new EmailValidator();
            ValidationResult validationResult;
            validator.Validate(emailAddress, out validationResult);

            Console.WriteLine("ReturnCode: " + validationResult.ReturnCode);
            Console.WriteLine("Message: " + validationResult.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
