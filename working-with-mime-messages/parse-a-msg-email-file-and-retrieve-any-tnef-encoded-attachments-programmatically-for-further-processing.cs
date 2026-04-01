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
            // Define input MSG file and output directory
            string inputMsgPath = "sample.msg";
            string outputDirectory = "TnefAttachments";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Placeholder",
                        "This is a placeholder MSG file."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Load the MSG file with TNEF attachment preservation
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveTnefAttachments = true
            };

            using (MailMessage message = MailMessage.Load(inputMsgPath, loadOptions))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    // Identify TNEF‑encoded attachments
                    if (attachment.IsTnef)
                    {
                        string attachmentName = attachment.Name;
                        if (string.IsNullOrEmpty(attachmentName))
                        {
                            attachmentName = "tnef_attachment.dat";
                        }

                        string outputPath = Path.Combine(outputDirectory, attachmentName);

                        try
                        {
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved TNEF attachment: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachmentName}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
