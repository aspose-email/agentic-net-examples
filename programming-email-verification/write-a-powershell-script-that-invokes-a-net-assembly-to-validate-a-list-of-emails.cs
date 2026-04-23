using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Tools.Verifications;

namespace EmailValidationSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "emails.txt";

                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                List<string> emailList = new List<string>();
                using (StreamReader reader = new StreamReader(inputPath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            emailList.Add(line.Trim());
                        }
                    }
                }

                EmailValidator validator = new EmailValidator();

                foreach (string email in emailList)
                {
                    ValidationResult result;
                    validator.Validate(email, out result);
                    Console.WriteLine($"{email}: {result.ReturnCode} - {result.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
