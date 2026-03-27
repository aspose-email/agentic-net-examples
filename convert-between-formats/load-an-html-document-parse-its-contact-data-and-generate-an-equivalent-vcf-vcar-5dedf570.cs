using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string htmlPath = "contacts.html";
            string vcfPath = "contacts.vcf";

            // Verify input HTML file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {htmlPath}");
                return;
            }

            string htmlContent;
            try
            {
                htmlContent = File.ReadAllText(htmlPath);
            }
            catch (Exception readEx)
            {
                Console.Error.WriteLine($"Error reading HTML file: {readEx.Message}");
                return;
            }

            // Simple regex extraction for name, email, and phone
            string namePattern = @"<span\s+class\s*=\s*[""']name[""']\s*>(?<name>[^<]+)</span>";
            string emailPattern = @"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}";
            string phonePattern = @"\b\d{3}[-.\s]?\d{3}[-.\s]?\d{4}\b";

            string name = string.Empty;
            string email = string.Empty;
            string phone = string.Empty;

            Match nameMatch = Regex.Match(htmlContent, namePattern, RegexOptions.IgnoreCase);
            if (nameMatch.Success)
            {
                name = nameMatch.Groups["name"].Value.Trim();
            }

            Match emailMatch = Regex.Match(htmlContent, emailPattern, RegexOptions.IgnoreCase);
            if (emailMatch.Success)
            {
                email = emailMatch.Value.Trim();
            }

            Match phoneMatch = Regex.Match(htmlContent, phonePattern);
            if (phoneMatch.Success)
            {
                phone = phoneMatch.Value.Trim();
            }

            // Create a contact and populate fields
            Contact contact = new Contact();
            if (!string.IsNullOrEmpty(name))
            {
                contact.DisplayName = name;
            }

            if (!string.IsNullOrEmpty(email))
            {
                contact.EmailAddresses.Add(new EmailAddress(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                // PhoneNumber is part of Aspose.Email.PersonalInfo
                contact.PhoneNumbers.Add(new PhoneNumber { Number = phone });
            }

            // Save the contact as a VCF file
            try
            {
                contact.Save(vcfPath);
                Console.WriteLine($"VCF file generated at: {vcfPath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Error saving VCF file: {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}