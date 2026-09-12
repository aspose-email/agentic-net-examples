using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.PersonalInfo;
using HtmlAgilityPack;

class Program
{
    static void Main()
    {
        try
        {
            // Define input HTML and output VCF file paths
            string htmlFilePath = "contact.html";
            string vcfFilePath = "contact.vcf";

            // Ensure the HTML input file exists; create a minimal placeholder if missing
            if (!File.Exists(htmlFilePath))
            {
                try
                {
                    string placeholderHtml = "<html><body>" +
                                            "<span class=\"name\">John Doe</span>" +
                                            "<span class=\"email\">john.doe@example.com</span>" +
                                            "</body></html>";
                    File.WriteAllText(htmlFilePath, placeholderHtml);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {ioEx.Message}");
                    return;
                }
            }

            // Load the HTML document using HtmlAgilityPack
            var htmlDoc = new HtmlDocument();
            try
            {
                htmlDoc.Load(htmlFilePath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Error loading HTML file: {loadEx.Message}");
                return;
            }

            // Extract name and email using XPath
            var nameNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class,'name')]");
            var emailNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class,'email')]");

            if (nameNode == null || emailNode == null)
            {
                Console.Error.WriteLine("Failed to locate required contact fields in the HTML.");
                return;
            }

            string fullName = nameNode.InnerText.Trim();
            string emailAddress = emailNode.InnerText.Trim();

            // Split full name into given name and surname (basic split on last space)
            string givenName = fullName;
            string surname = string.Empty;
            int lastSpaceIndex = fullName.LastIndexOf(' ');
            if (lastSpaceIndex > 0 && lastSpaceIndex < fullName.Length - 1)
            {
                givenName = fullName.Substring(0, lastSpaceIndex);
                surname = fullName.Substring(lastSpaceIndex + 1);
            }

            // Create a Contact object and populate fields
            var contact = new Contact
            {
                GivenName = givenName,
                Surname = surname
            };
            contact.EmailAddresses.Add(new EmailAddress(emailAddress));

            // Save the contact as a VCF file using the default VCard format
            try
            {
                contact.Save(vcfFilePath);
                Console.WriteLine($"VCF file generated successfully at: {vcfFilePath}");
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
