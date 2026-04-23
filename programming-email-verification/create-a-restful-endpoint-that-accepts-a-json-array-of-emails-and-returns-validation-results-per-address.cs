using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email.Tools.Verifications;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Determine input JSON file path (optional command‑line argument)
            string inputPath = "emails.json";
            if (args.Length > 0)
            {
                inputPath = args[0];
            }

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "[]");
                    Console.Error.WriteLine($"Input file not found. Created placeholder at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Read the JSON content safely
            string jsonContent;
            try
            {
                jsonContent = File.ReadAllText(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read input file: {ex.Message}");
                return;
            }

            // Deserialize the JSON array of email strings
            List<string> emailList;
            try
            {
                emailList = JsonSerializer.Deserialize<List<string>>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to parse JSON: {ex.Message}");
                return;
            }

            if (emailList == null || emailList.Count == 0)
            {
                Console.Error.WriteLine("No email addresses found to validate.");
                return;
            }

            // Validate each email address using Aspose.Email's EmailValidator
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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
