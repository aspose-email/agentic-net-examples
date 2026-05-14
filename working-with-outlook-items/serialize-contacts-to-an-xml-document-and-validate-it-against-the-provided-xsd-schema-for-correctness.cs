using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            string xmlPath = "contacts.xml";
            string xsdPath = "contacts.xsd";

            // Ensure output directories exist
            EnsureDirectoryExists(xmlPath);
            EnsureDirectoryExists(xsdPath);

            // Ensure XSD exists; create a minimal placeholder if missing
            if (!File.Exists(xsdPath))
            {
                string placeholderXsd = @"<?xml version=""1.0"" encoding=""utf-8""?>
<xs:schema xmlns:xs=""http://www.w3.org/2001/XMLSchema"">
  <xs:element name=""Contacts"">
    <xs:complexType>
      <xs:sequence>
        <xs:element name=""Contact"" maxOccurs=""unbounded"">
          <xs:complexType>
            <xs:sequence>
              <xs:element name=""DisplayName"" type=""xs:string"" minOccurs=""0""/>
              <xs:element name=""Email"" type=""xs:string"" minOccurs=""0""/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>";
                try
                {
                    File.WriteAllText(xsdPath, placeholderXsd);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder XSD: {ex.Message}");
                    return;
                }
            }

            // Create sample contacts
            List<Contact> contacts = new List<Contact>();

            Contact contact1 = new Contact
            {
                DisplayName = "John Doe"
            };
            contact1.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));
            contacts.Add(contact1);

            Contact contact2 = new Contact
            {
                DisplayName = "Jane Smith"
            };
            contact2.EmailAddresses.Add(new EmailAddress("jane.smith@example.com"));
            contacts.Add(contact2);

            // Serialize contacts to XML
            try
            {
                using (FileStream fs = new FileStream(xmlPath, FileMode.Create, FileAccess.Write))
                using (XmlWriter writer = XmlWriter.Create(fs, new XmlWriterSettings { Indent = true }))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Contacts");

                    foreach (Contact c in contacts)
                    {
                        writer.WriteStartElement("Contact");

                        if (!string.IsNullOrEmpty(c.DisplayName))
                        {
                            writer.WriteElementString("DisplayName", c.DisplayName);
                        }

                        foreach (EmailAddress email in c.EmailAddresses)
                        {
                            if (!string.IsNullOrEmpty(email.Address))
                            {
                                writer.WriteElementString("Email", email.Address);
                            }
                        }

                        writer.WriteEndElement(); // Contact
                    }

                    writer.WriteEndElement(); // Contacts
                    writer.WriteEndDocument();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing XML file: {ex.Message}");
                return;
            }

            // Validate the generated XML against the XSD
            try
            {
                XmlSchemaSet schemas = new XmlSchemaSet();
                using (FileStream xsdStream = new FileStream(xsdPath, FileMode.Open, FileAccess.Read))
                {
                    schemas.Add(null, XmlReader.Create(xsdStream));
                }

                XmlReaderSettings settings = new XmlReaderSettings
                {
                    ValidationType = ValidationType.Schema,
                    Schemas = schemas
                };
                settings.ValidationEventHandler += (sender, args) =>
                {
                    Console.Error.WriteLine($"Validation {args.Severity}: {args.Message}");
                };

                using (FileStream xmlStream = new FileStream(xmlPath, FileMode.Open, FileAccess.Read))
                using (XmlReader reader = XmlReader.Create(xmlStream, settings))
                {
                    while (reader.Read()) { }
                }

                Console.WriteLine("XML validation completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during XML validation: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void EnsureDirectoryExists(string filePath)
    {
        try
        {
            string directory = Path.GetDirectoryName(Path.GetFullPath(filePath));
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to ensure directory for '{filePath}': {ex.Message}");
            throw;
        }
    }
}
