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
            string inputPath = "email.mhtml";
            string outputPath = "contact.vcf";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Load the MHTML message with load options
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath, new MhtmlLoadOptions());
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MHTML: {ex.Message}");
                return;
            }

            using (message)
            {
                string htmlBody = message.HtmlBody;
                if (string.IsNullOrEmpty(htmlBody))
                {
                    Console.Error.WriteLine("Message does not contain an HTML body.");
                    return;
                }

                // Parse HTML to extract contact fields
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(htmlBody);

                var nameNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class,'name')]");
                var emailNode = htmlDoc.DocumentNode.SelectSingleNode("//a[starts-with(@href,'mailto:')]");
                var phoneNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class,'phone')]");

                var contact = new Contact();

                // Extract name
                if (nameNode != null)
                {
                    string fullName = nameNode.InnerText.Trim();
                    var nameParts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (nameParts.Length > 0) contact.GivenName = nameParts[0];
                    if (nameParts.Length > 1) contact.Surname = nameParts[nameParts.Length - 1];
                    if (nameParts.Length > 2)
                        contact.MiddleName = string.Join(" ", nameParts, 1, nameParts.Length - 2);
                }

                // Extract email
                if (emailNode != null)
                {
                    string href = emailNode.GetAttributeValue("href", string.Empty);
                    string email = href.Replace("mailto:", string.Empty).Trim();
                    if (!string.IsNullOrEmpty(email))
                        contact.EmailAddresses.Add(new EmailAddress(email));
                }

                // Extract phone
                if (phoneNode != null)
                {
                    string phone = phoneNode.InnerText.Trim();
                    if (!string.IsNullOrEmpty(phone))
                        contact.PhoneNumbers.Add(new PhoneNumber { Number = phone, Category = PhoneNumberCategory.Company });
                }

                // Save contact as VCF
                try
                {
                    contact.Save(outputPath);
                    Console.WriteLine($"Contact saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save VCF: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
