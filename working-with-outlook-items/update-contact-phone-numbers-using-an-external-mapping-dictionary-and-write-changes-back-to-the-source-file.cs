using Aspose.Email.PersonalInfo;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "contact.msg";
            string outputPath = "contact_updated.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiContact placeholder = new MapiContact())
                    {
                        placeholder.NameInfo = new MapiContactNamePropertySet
                        {
                            DisplayName = "Placeholder Contact"
                        };
                        placeholder.Telephones = new MapiContactTelephonePropertySet
                        {
                            BusinessTelephoneNumber = "+1-000-0000"
                        };
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder contact created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder contact: {ex.Message}");
                    return;
                }
            }

            // Load the existing contact.
            MapiContact contact;
            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    if (msg.SupportedType != MapiItemType.Contact)
                    {
                        Console.Error.WriteLine("The provided file is not a contact message.");
                        return;
                    }
                    contact = (MapiContact)msg.ToMapiMessageItem();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load contact: {ex.Message}");
                return;
            }

            // Mapping of telephone property names to new values.
            var phoneMapping = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "BusinessTelephoneNumber", "+1-555-1111" },
                { "HomeTelephoneNumber", "+1-555-2222" },
                { "MobileTelephoneNumber", "+1-555-3333" }
            };

            // Update telephone numbers based on the mapping.
            MapiContactTelephonePropertySet phones = contact.Telephones;
            foreach (var kvp in phoneMapping)
            {
                switch (kvp.Key)
                {
                    case "BusinessTelephoneNumber":
                        phones.BusinessTelephoneNumber = kvp.Value;
                        break;
                    case "HomeTelephoneNumber":
                        phones.HomeTelephoneNumber = kvp.Value;
                        break;
                    case "MobileTelephoneNumber":
                        phones.MobileTelephoneNumber = kvp.Value;
                        break;
                    case "AssistantTelephoneNumber":
                        phones.AssistantTelephoneNumber = kvp.Value;
                        break;
                    case "CarTelephoneNumber":
                        phones.CarTelephoneNumber = kvp.Value;
                        break;
                    case "CompanyMainTelephoneNumber":
                        phones.CompanyMainTelephoneNumber = kvp.Value;
                        break;
                    case "Home2TelephoneNumber":
                        phones.Home2TelephoneNumber = kvp.Value;
                        break;
                    case "Business2TelephoneNumber":
                        phones.Business2TelephoneNumber = kvp.Value;
                        break;
                    case "OtherTelephoneNumber":
                        phones.OtherTelephoneNumber = kvp.Value;
                        break;
                    case "PagerTelephoneNumber":
                        phones.PagerTelephoneNumber = kvp.Value;
                        break;
                    case "PrimaryTelephoneNumber":
                        phones.PrimaryTelephoneNumber = kvp.Value;
                        break;
                    case "RadioTelephoneNumber":
                        phones.RadioTelephoneNumber = kvp.Value;
                        break;
                    case "TelexNumber":
                        phones.TelexNumber = kvp.Value;
                        break;
                    case "TtyTddPhoneNumber":
                        phones.TtyTddPhoneNumber = kvp.Value;
                        break;
                    // Add additional cases as needed.
                }
            }

            // Save the updated contact back to a file.
            try
            {
                // Ensure the directory for the output file exists.
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the contact as a MSG file.
                contact.Save(outputPath);
                Console.WriteLine($"Contact updated and saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save updated contact: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
