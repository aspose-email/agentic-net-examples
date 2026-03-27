using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string sourcePath = "source.eml";
            string targetPath = "target.eml";

            // Verify that the source file exists before attempting to load it
            if (!File.Exists(sourcePath))
            {
                Console.Error.WriteLine($"Error: File not found – {sourcePath}");
                return;
            }

            // Load the email message from the EML file
            using (Aspose.Email.MailMessage mailMessage = Aspose.Email.MailMessage.Load(sourcePath))
            {
                // Configure save options to preserve embedded message format
                Aspose.Email.EmlSaveOptions saveOptions = new Aspose.Email.EmlSaveOptions(Aspose.Email.MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };

                // Save the message to a new EML file with the specified options
                mailMessage.Save(targetPath, saveOptions);
                Console.WriteLine($"Message saved to {targetPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}