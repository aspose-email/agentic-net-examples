using Aspose.Email;
using System;
using System.Threading;
using Aspose.Email.Tools.Verifications;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string emailAddress = "test@example.com";
                const int maxRetryAttempts = 3;
                const int retryDelayMilliseconds = 2000;

                int attempt = 0;
                ValidationResult validationResult;
                EmailValidator validator = new EmailValidator();

                while (true)
                {
                    attempt++;
                    validator.Validate(emailAddress, out validationResult);

                    // If the validation succeeded or we have exhausted retries, exit loop
                    if (validationResult.ReturnCode != ValidationResponseCode.MailServerValidationError ||
                        attempt >= maxRetryAttempts)
                    {
                        break;
                    }

                    Console.WriteLine($"Attempt {attempt} failed with MailServerValidationError. Retrying in {retryDelayMilliseconds} ms...");
                    Thread.Sleep(retryDelayMilliseconds);
                }

                Console.WriteLine($"Validation completed. Return code: {validationResult.ReturnCode}");
                Console.WriteLine($"Message: {validationResult.Message}");
                if (validationResult.LastException != null)
                {
                    Console.WriteLine($"Exception: {validationResult.LastException.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
