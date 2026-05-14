using Aspose.Email.PersonalInfo;
using System;
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
            string outputPath = "contact_with_custom.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the MSG file safely
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Ensure the MSG is a contact
                if (msg.SupportedType != MapiItemType.Contact)
                {
                    Console.Error.WriteLine("The provided MSG file is not a contact.");
                    return;
                }

                // Convert to MapiContact
                MapiContact contact = (MapiContact)msg.ToMapiMessageItem();

                // Retrieve custom properties from the original message
                MapiPropertyCollection customProps = msg.GetCustomProperties();

                int fieldIndex = 1;
                foreach (var entry in customProps)
                {
                    // Access the property value
                    object value = entry.Value;
                    if (value == null) continue;

                    string stringValue = value.ToString();

                    // Map up to four custom properties to UserField1‑UserField4
                    switch (fieldIndex)
                    {
                        case 1:
                            contact.OtherFields.UserField1 = stringValue;
                            break;
                        case 2:
                            contact.OtherFields.UserField2 = stringValue;
                            break;
                        case 3:
                            contact.OtherFields.UserField3 = stringValue;
                            break;
                        case 4:
                            contact.OtherFields.UserField4 = stringValue;
                            break;
                        default:
                            // Additional custom properties can be handled here if needed
                            break;
                    }

                    fieldIndex++;
                    if (fieldIndex > 4) break;
                }

                // Save the modified contact back to MSG
                try
                {
                    contact.Save(outputPath);
                    Console.WriteLine($"Contact saved with custom fields to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save contact: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
