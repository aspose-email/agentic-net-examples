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
            string inputMsgPath = "input.msg";
            string outputMboxPath = "output.mbox";

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
            string outputDir = Path.GetDirectoryName(outputMboxPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                // Convert to MailMessage
                MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                // Prepare MBOX writer
                MboxSaveOptions saveOptions = new MboxSaveOptions();
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(outputMboxPath, saveOptions))
                {
                    // Write the MailMessage to the MBOX file
                    writer.WriteMessage(mailMessage);
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
