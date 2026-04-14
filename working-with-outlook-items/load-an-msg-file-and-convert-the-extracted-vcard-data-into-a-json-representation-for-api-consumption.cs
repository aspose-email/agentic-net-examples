using System;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.PersonalInfo.VCard;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgFilePath = "input.msg";

            // Output JSON file path
            string jsonOutputPath = "output.json";

            // Verify that the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"The file '{msgFilePath}' does not exist.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(msgFilePath))
            {
                // Find the first attachment that looks like a vCard
                MapiAttachment vcardAttachment = null;
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    if (attachment.FileName != null && attachment.FileName.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                    {
                        vcardAttachment = attachment;
                        break;
                    }
                }

                if (vcardAttachment == null)
                {
                    Console.Error.WriteLine("No vCard attachment found in the MSG file.");
                    return;
                }

                // Load the vCard from the attachment stream
                using (MemoryStream vcardStream = new MemoryStream())
                {
                    vcardAttachment.Save(vcardStream);
                    vcardStream.Position = 0;

                    VCardContact vcardContact = VCardContact.Load(vcardStream);

                    // Serialize the VCardContact to JSON
                    JsonSerializerOptions jsonOptions = new JsonSerializerOptions
                    {
                        WriteIndented = true
                    };
                    string json = JsonSerializer.Serialize(vcardContact, jsonOptions);

                    // Ensure the output directory exists
                    string outputDirectory = Path.GetDirectoryName(jsonOutputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    // Write JSON to file
                    try
                    {
                        File.WriteAllText(jsonOutputPath, json);
                        Console.WriteLine($"vCard data has been written to '{jsonOutputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
    }
}
