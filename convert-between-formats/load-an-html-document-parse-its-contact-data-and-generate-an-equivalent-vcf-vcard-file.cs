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
            string inputHtmlPath = "contacts.html";
            string outputVcfPath = "contacts.vcf";

            if (!File.Exists(inputHtmlPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputHtmlPath}");
                return;
            }

            string htmlContent;
            using (StreamReader reader = new StreamReader(inputHtmlPath))
            {
                htmlContent = reader.ReadToEnd();
            }

            // Simple regex to extract name and email from HTML.
            // Expected format: <span class="name">John Doe</span> and <span class="email">john@example.com</span>
            string namePattern = @"<span\s+class\s*=\s*[""']name[""']\s*>(?<name>[^<]+)</span>";
            string emailPattern = @"<span\s+class\s*=\s*[""']email[""']\s*>(?<email>[^<]+)</span>";

            Match nameMatch = Regex.Match(htmlContent, namePattern, RegexOptions.IgnoreCase);
            Match emailMatch = Regex.Match(htmlContent, emailPattern, RegexOptions.IgnoreCase);

            if (!nameMatch.Success || !emailMatch.Success)
            {
                Console.Error.WriteLine("Error: Unable to parse contact information from HTML.");
                return;
            }

            string contactName = nameMatch.Groups["name"].Value.Trim();
            string contactEmail = emailMatch.Groups["email"].Value.Trim();

            Contact contact = new Contact();
            contact.DisplayName = contactName;
            contact.EmailAddresses.Add(new EmailAddress(contactEmail));

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(outputVcfPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {dirEx.Message}");
                    return;
                }
            }

            try
            {
                contact.Save(outputVcfPath);
                Console.WriteLine($"VCF file saved to {outputVcfPath}");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Error: Failed to save VCF – {saveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
