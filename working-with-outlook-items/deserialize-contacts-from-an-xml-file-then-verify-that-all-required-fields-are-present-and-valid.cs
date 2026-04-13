using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string xmlPath = "contacts.xml";

            // Ensure the XML file exists; create a minimal placeholder if it does not.
            if (!File.Exists(xmlPath))
            {
                try
                {
                    string placeholderXml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<Contacts>
    <Contact>
        <DisplayName>John Doe</DisplayName>
        <Email>john.doe@example.com</Email>
        <Phone>+1234567890</Phone>
    </Contact>
</Contacts>";
                    File.WriteAllText(xmlPath, placeholderXml);
                    Console.WriteLine($"Placeholder XML file created at '{xmlPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder XML file: {ex.Message}");
                    return;
                }
            }

            List<Contact> contacts = new List<Contact>();

            // Load and parse the XML file.
            try
            {
                using (FileStream fileStream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
                {
                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.Load(fileStream);

                    XmlNodeList contactNodes = xmlDoc.SelectNodes("/Contacts/Contact");
                    if (contactNodes != null)
                    {
                        foreach (XmlNode contactNode in contactNodes)
                        {
                            // Extract required fields.
                            string displayName = string.Empty;
                            string email = string.Empty;
                            string phone = string.Empty;

                            XmlNode nameNode = contactNode.SelectSingleNode("DisplayName");
                            if (nameNode != null)
                                displayName = nameNode.InnerText.Trim();

                            XmlNode emailNode = contactNode.SelectSingleNode("Email");
                            if (emailNode != null)
                                email = emailNode.InnerText.Trim();

                            XmlNode phoneNode = contactNode.SelectSingleNode("Phone");
                            if (phoneNode != null)
                                phone = phoneNode.InnerText.Trim();

                            // Validate required fields.
                            bool isValid = true;
                            if (string.IsNullOrEmpty(displayName))
                            {
                                Console.Error.WriteLine("Contact missing DisplayName.");
                                isValid = false;
                            }

                            if (string.IsNullOrEmpty(email) || !email.Contains("@"))
                            {
                                Console.Error.WriteLine($"Contact '{displayName}' has invalid or missing Email.");
                                isValid = false;
                            }

                            if (!isValid)
                                continue; // Skip invalid contact.

                            // Create Aspose.Email Contact object.
                            Contact asposeContact = new Contact();
                            asposeContact.DisplayName = displayName;
                            asposeContact.EmailAddresses.Add(new EmailAddress(email));

                            if (!string.IsNullOrEmpty(phone))
                            {
                                // Add phone number as a generic phone entry.
                                asposeContact.PhoneNumbers.Add(new PhoneNumber { Number = phone, Category = PhoneNumberCategory.Company });
                            }

                            contacts.Add(asposeContact);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading or parsing XML file: {ex.Message}");
                return;
            }

            Console.WriteLine($"Successfully loaded {contacts.Count} valid contact(s).");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
