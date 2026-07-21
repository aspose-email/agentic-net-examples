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
            // Input MSG file path
            string inputMsgPath = "input.msg";
            // Output EML file path
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
            string outputDir = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            MapiMessage mapiMsg = MapiMessage.Load(inputMsgPath);

            // Convert to MailMessage for easier manipulation
            MailConversionOptions conversionOpts = new MailConversionOptions();
            using (MailMessage mailMsg = mapiMsg.ToMailMessage(conversionOpts))
            {
                // Create a new attachment (ensure the file exists or adjust path as needed)
                string attachmentPath = "newfile.txt";
                if (!File.Exists(attachmentPath))
                {
                    // Create a minimal placeholder file if missing
                    File.WriteAllText(attachmentPath, "Placeholder content");
                }

                using (Attachment newAttachment = new Attachment(attachmentPath))
                {
                    // Add the attachment to the email
                    mailMsg.AddAttachment(newAttachment);
                }

                // Save the modified message to disk
                mailMsg.Save(outputEmlPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
