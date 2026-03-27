using System;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            string[] emails = new string[] { "valid@example.com", "invalid-email", "test@domain" };
            Aspose.Email.Tools.Verifications.EmailValidator validator = new Aspose.Email.Tools.Verifications.EmailValidator();

            foreach (string email in emails)
            {
                Aspose.Email.Tools.Verifications.ValidationResult result;
                validator.Validate(email, out result);

                if (result.ReturnCode == Aspose.Email.Tools.Verifications.ValidationResponseCode.ValidationSuccess)
                {
                    Console.WriteLine($"'{email}' is valid.");
                }
                else
                {
                    Console.WriteLine($"'{email}' is invalid. Reason: {result.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}