using Aspose.Email;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main()
    {
        try
        {
            const int addressCount = 10000;
            List<string> emailAddresses = new List<string>(addressCount);
            for (int i = 0; i < addressCount; i++)
            {
                emailAddresses.Add($"user{i}@example.com");
            }

            EmailValidator validator = new EmailValidator();
            validator.Timeout = 5000; // milliseconds, optional

            Stopwatch timer = Stopwatch.StartNew();
            int successfulValidations = 0;

            foreach (string address in emailAddresses)
            {
                ValidationResult result;
                validator.Validate(address, ValidationPolicy.SyntaxOnly, out result);
                if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                {
                    successfulValidations++;
                }
            }

            timer.Stop();

            Console.WriteLine($"Validated {addressCount} email addresses in {timer.ElapsedMilliseconds} ms.");
            Console.WriteLine($"Successful validations: {successfulValidations}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
