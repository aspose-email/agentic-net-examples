using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Set conversion options with a handler to preserve calendar attachments
            MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
            conversionOptions.MessageHandler = (MailMessage message) =>
            {
                // Detect calendar attachments (Content-Type: text/calendar)
                foreach (Attachment attachment in message.Attachments)
                {
                    if (attachment.ContentType.MediaType.Equals("text/calendar", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Preserving calendar attachment in message: {message.Subject}");
                        // No further action required; the conversion retains the attachment.
                    }
                }
            };

            // Perform the conversion
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, conversionOptions))
            {
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
