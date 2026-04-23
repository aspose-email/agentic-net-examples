using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string oftFilePath = "template.oft";
            string emlFilePath = "output.eml";

            // Verify the OFT input file exists
            if (!File.Exists(oftFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(oftFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {oftFilePath}");
                return;
            }

            // Load the OFT file into a MailMessage
            using (MailMessage message = MailMessage.Load(oftFilePath))
            {
                // Configure save options to preserve embedded message formats
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Save the message as EML, preserving attachments and original timestamps
                message.Save(emlFilePath, saveOptions);
                Console.WriteLine($"Successfully converted '{oftFilePath}' to '{emlFilePath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
