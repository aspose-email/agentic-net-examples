using Aspose.Email;
using System;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Enter email addresses to validate (empty line to exit):");
                while (true)
                {
                    Console.Write("Email: ");
                    string input = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        break;
                    }

                    EmailValidator validator = new EmailValidator();
                    ValidationResult result;
                    validator.Validate(input, out result);

                    if (result.ReturnCode == ValidationResponseCode.ValidationSuccess)
                    {
                        Console.WriteLine("Valid email address.");
                    }
                    else
                    {
                        Console.WriteLine($"Invalid email address. Reason: {result.Message}");
                        if (result.LastException != null)
                        {
                            Console.WriteLine($"Exception: {result.LastException.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
