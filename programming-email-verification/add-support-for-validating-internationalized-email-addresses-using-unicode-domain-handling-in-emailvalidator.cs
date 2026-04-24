using System;
using Aspose.Email;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            // Create an instance of EmailValidator
            EmailValidator validator = new EmailValidator();

            // Internationalized email address (Unicode domain)
            string email = "用户@例子.测试";

            // Perform validation using syntax and domain policy
            ValidationResult result;
            validator.Validate(email, ValidationPolicy.SyntaxAndDomain, out result);

            // Output the validation return code and message
            Console.WriteLine("ReturnCode: " + result.ReturnCode);
            Console.WriteLine("Message: " + result.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
