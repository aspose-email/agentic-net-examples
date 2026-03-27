using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string email = "example@example.com";

            EmailValidator validator = new EmailValidator();
            ValidationResult result;
            validator.Validate(email, out result);

            Console.WriteLine("ReturnCode: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
