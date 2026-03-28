using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            string htmlPath = "contact.html";

            // Verify input file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Input file '{htmlPath}' not found.");
                return;
            }

            // Read HTML content safely
            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Failed to read file: {readEx.Message}");
                return;
            }

            // Simple extraction of name, email, and phone using regex
            string name = ExtractPattern(htmlContent, @"<h1>(.*?)</h1>");
            string email = ExtractPattern(htmlContent, @"mailto:([^\"">]+)");
            string phone = ExtractPattern(htmlContent, @"Phone:\s*([+\d\s\-\(\)]+)");

            // Create a Contact object and populate fields
            Contact contact = new Contact();

            if (!string.IsNullOrEmpty(name))
                contact.DisplayName = name;

            if (!string.IsNullOrEmpty(email))
                contact.EmailAddresses.Add(new EmailAddress(email));

            if (!string.IsNullOrEmpty(phone))
            {
                PhoneNumber phoneNumber = new PhoneNumber();
                phoneNumber.Number = phone.Trim();
                contact.PhoneNumbers.Add(phoneNumber);
            }

            // Output VCF file path
            string outputPath = "contact.vcf";

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            // Save the contact as a VCF file
            try
            {
                contact.Save(outputPath);
                Console.WriteLine($"VCard saved to {outputPath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save VCard: {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper method to extract the first capturing group from a regex pattern
    static string ExtractPattern(string input, string pattern)
    {
        Match match = Regex.Match(input, pattern, RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }
}
