using Aspose.Email;
using System;
using System.IO;
using System.Xml.Linq;
using System.Collections.Generic;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string xmlPath = "contacts.xml";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(xmlPath))
            {
                var placeholder = new XDocument(
                    new XElement("Contacts",
                        new XElement("Contact",
                            new XElement("DisplayName", "John Doe"),
                            new XElement("Email", "john.doe@example.com")
                        )
                    )
                );
                placeholder.Save(xmlPath);
                Console.WriteLine($"Placeholder file created at '{xmlPath}'.");
            }

            // Load XML safely
            XDocument doc;
            try
            {
                using (FileStream fs = new FileStream(xmlPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    doc = XDocument.Load(fs);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load XML file: {ex.Message}");
                return;
            }

            // Parse contacts
            IEnumerable<XElement> contactElements = doc.Root?.Elements("Contact");
            if (contactElements == null)
            {
                Console.Error.WriteLine("No contacts found in the XML.");
                return;
            }

            List<Contact> contacts = new List<Contact>();
            foreach (XElement element in contactElements)
            {
                Contact contact = new Contact();

                // DisplayName (required)
                XElement nameElem = element.Element("DisplayName");
                if (nameElem != null)
                {
                    contact.DisplayName = nameElem.Value.Trim();
                }

                // Email (required)
                XElement emailElem = element.Element("Email");
                if (emailElem != null)
                {
                    string email = emailElem.Value.Trim();
                    if (!string.IsNullOrEmpty(email))
                    {
                        contact.EmailAddresses.Add(new EmailAddress(email));
                    }
                }

                // Optional fields can be parsed similarly (e.g., PhoneNumbers, CompanyName, etc.)

                contacts.Add(contact);
            }

            // Validate contacts
            foreach (Contact contact in contacts)
            {
                bool isValid = true;

                // Validate DisplayName
                if (string.IsNullOrWhiteSpace(contact.DisplayName))
                {
                    Console.WriteLine("Invalid contact: DisplayName is missing.");
                    isValid = false;
                }

                // Validate at least one email address and its format
                if (contact.EmailAddresses.Count == 0)
                {
                    Console.WriteLine($"Invalid contact '{contact.DisplayName}': No email address provided.");
                    isValid = false;
                }
                else
                {
                    foreach (EmailAddress emailAddr in contact.EmailAddresses)
                    {
                        string email = emailAddr.Address;
                        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
                        {
                            Console.WriteLine($"Invalid contact '{contact.DisplayName}': Email '{email}' is not valid.");
                            isValid = false;
                        }
                    }
                }

                if (isValid)
                {
                    Console.WriteLine($"Contact '{contact.DisplayName}' is valid.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
