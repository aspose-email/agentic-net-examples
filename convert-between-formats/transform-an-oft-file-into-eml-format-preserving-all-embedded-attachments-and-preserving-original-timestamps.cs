using System;
using System.IO;
using Aspose.Email;

// Author: Aspose.Email example - Convert OFT to EML preserving attachments and timestamps
class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "template.oft";
            const string outputPath = "output.eml";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the Outlook template (OFT) into a MailMessage
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save as EML; default options preserve attachments and original timestamps
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                message.Save(outputPath, emlSaveOptions);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
