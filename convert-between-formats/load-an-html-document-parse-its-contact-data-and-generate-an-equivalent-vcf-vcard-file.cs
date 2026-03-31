using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.PersonalInfo;
using HtmlAgilityPack;

class Program
{
    static void Main()
    {
        try
        {
            string htmlPath = "contact.html";
            string vcfPath = "contact.vcf";

            // Verify input HTML file exists
            if (!File.Exists(htmlPath))
            {
                Console.Error.WriteLine($"Input file not found: {htmlPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(vcfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load and parse HTML
            var htmlDoc = new HtmlDocument();
            htmlDoc.Load(htmlPath);

            // Extract name (example XPath, adjust as needed)
            var nameNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='name']");
            string fullName = nameNode?.InnerText?.Trim() ?? "Unknown";

            // Extract email (first mailto link)
            var emailNode = htmlDoc.DocumentNode.SelectSingleNode("//a[starts-with(@href,'mailto:')]");
            string email = null;
            if (emailNode != null)
            {
                string href = emailNode.GetAttributeValue("href", string.Empty);
                if (href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                {
                    email = href.Substring(7);
                }
            }

            // Extract phone (example XPath)
            var phoneNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='phone']");
            string phone = phoneNode?.InnerText?.Trim();

            // Create Aspose.Email contact
            var contact = new Contact
            {
                DisplayName = fullName
            };

            // Populate given name and surname if possible
            var nameParts = fullName.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (nameParts.Length > 0) contact.GivenName = nameParts[0];
            if (nameParts.Length > 1) contact.Surname = nameParts[1];

            // Add email address (typed)
            if (!string.IsNullOrEmpty(email))
            {
                contact.EmailAddresses.Add(new EmailAddress(email));
            }

            // Add phone number (typed)
            if (!string.IsNullOrEmpty(phone))
            {
                contact.PhoneNumbers.Add(new PhoneNumber
                {
                    Number = phone,
                    Category = PhoneNumberCategory.Work
                });
            }

            // Save as VCF
            contact.Save(vcfPath);
            Console.WriteLine($"Contact saved to {vcfPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
