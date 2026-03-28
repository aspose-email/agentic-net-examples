using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "input.msg";
            string mboxPath = "output.mbox";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(mboxPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load MSG file into a MapiMessage and convert to MailMessage
            using (MapiMessage mapiMsg = MapiMessage.Load(msgPath))
            {
                MailConversionOptions convOptions = new MailConversionOptions();
                using (MailMessage mailMsg = mapiMsg.ToMailMessage(convOptions))
                {
                    // Write the MailMessage to an MBOX file using MboxrdStorageWriter
                    MboxSaveOptions saveOptions = new MboxSaveOptions();
                    using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, saveOptions))
                    {
                        writer.WriteMessage(mailMsg);
                    }
                }
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
