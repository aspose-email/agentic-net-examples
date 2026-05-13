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
            // Define input MSG file and output EML file paths
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                // Prepare conversion options (preserve embedded message format)
                MailConversionOptions conversionOptions = new MailConversionOptions();

                // Convert to MailMessage
                using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                {
                    // Save as EML using EmlSaveOptions
                    EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                    mail.Save(outputEmlPath, emlSaveOptions);

                    // Verify that all original attachments are present
                    int originalAttachmentCount = msg.Attachments.Count;
                    int convertedAttachmentCount = mail.Attachments.Count;

                    if (originalAttachmentCount == convertedAttachmentCount)
                    {
                        Console.WriteLine("Conversion successful. All attachments are preserved.");
                    }
                    else
                    {
                        Console.WriteLine($"Conversion completed, but attachment count differs. Original: {originalAttachmentCount}, Converted: {convertedAttachmentCount}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
