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
            string outputPath = "contact.vcf";

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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file as a MAPI message
            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
            {
                // Verify the MSG is a contact
                if (mapiMessage.SupportedType != MapiItemType.Contact)
                {
                    Console.Error.WriteLine("The provided MSG file is not a contact.");
                    return;
                }

                // Convert to MapiContact to access contact-specific members
                using (MapiContact contact = (MapiContact)mapiMessage.ToMapiMessageItem())
                {
                    // Use default save options which preserve original MAPI properties
                    MapiContactSaveOptions saveOptions = MapiContactSaveOptions.Default;

                    // Save the contact to VCard format
                    contact.Save(outputPath, saveOptions);
                }
            }

            Console.WriteLine("Contact saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
