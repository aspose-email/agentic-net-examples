using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MSG file path and output directory
            string inputMsgPath = "input.msg";
            string outputDirectory = "ExtractedAttachments";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                // Optionally create a minimal placeholder MSG to avoid failure
                using (MailMessage placeholder = new MailMessage())
                {
                    placeholder.Save(inputMsgPath, SaveOptions.DefaultMsg);
                }
                Console.Error.WriteLine("Created placeholder MSG file.");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load MSG with TNEF attachment extraction enabled
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputMsgPath, loadOptions))
            {
                int attachmentIndex = 0;
                foreach (Attachment attachment in message.Attachments)
                {
                    // Determine a safe file name for the attachment
                    string attachmentName = attachment.Name;
                    if (string.IsNullOrEmpty(attachmentName))
                    {
                        attachmentName = $"attachment_{attachmentIndex}";
                    }

                    // Sanitize file name (remove invalid path characters)
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        attachmentName = attachmentName.Replace(invalidChar, '_');
                    }

                    string outputPath = Path.Combine(outputDirectory, attachmentName);

                    // Save attachment content to file
                    try
                    {
                        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            if (attachment.ContentStream != null)
                            {
                                attachment.ContentStream.CopyTo(fileStream);
                            }
                        }
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachmentName}': {ex.Message}");
                    }

                    attachmentIndex++;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
