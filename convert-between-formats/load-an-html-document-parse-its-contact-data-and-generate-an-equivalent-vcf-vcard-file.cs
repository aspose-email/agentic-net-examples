using System;
using System.IO;
using HtmlAgilityPack;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Mapi;

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

            // Load HTML document
            HtmlDocument htmlDoc = new HtmlDocument();
            try
            {
                htmlDoc.Load(htmlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load HTML file: {ex.Message}");
                return;
            }

            // Extract contact fields using XPath
            string givenName = string.Empty;
            string surname = string.Empty;
            string email = string.Empty;
            string phone = string.Empty;

            HtmlNode nameNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='name']");
            if (nameNode != null)
            {
                string[] nameParts = nameNode.InnerText.Trim().Split(' ');
                if (nameParts.Length > 0) givenName = nameParts[0];
                if (nameParts.Length > 1) surname = nameParts[nameParts.Length - 1];
            }

            HtmlNode emailNode = htmlDoc.DocumentNode.SelectSingleNode("//a[starts-with(@href,'mailto:')]");
            if (emailNode != null)
            {
                string href = emailNode.GetAttributeValue("href", string.Empty);
                if (href.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
                {
                    email = href.Substring(7);
                }
            }

            HtmlNode phoneNode = htmlDoc.DocumentNode.SelectSingleNode("//span[@class='phone']");
            if (phoneNode != null)
            {
                phone = phoneNode.InnerText.Trim();
            }

            // Create Aspose.Email contact
            Contact contact = new Contact();
            contact.GivenName = givenName;
            contact.Surname = surname;

            if (!string.IsNullOrEmpty(email))
            {
                contact.EmailAddresses.Add(new EmailAddress(email));
            }

            if (!string.IsNullOrEmpty(phone))
            {
                PhoneNumber phoneNumber = new PhoneNumber();
                phoneNumber.Number = phone;
                phoneNumber.Category = PhoneNumberCategory.Work;
                contact.PhoneNumbers.Add(phoneNumber);
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(vcfPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Save contact as VCF
            try
            {
                contact.Save(vcfPath, ContactSaveFormat.VCard);
                Console.WriteLine($"Contact saved to {vcfPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save VCF file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
