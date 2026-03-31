using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Paths
            string inputMsgPath = "sample.msg";
            string outputDir = "output";
            string modifiedMsgPath = "modified.msg";

            // Ensure input MSG exists; create minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder Subject", "Placeholder body");
                    placeholder.Save(inputMsgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error preparing output directory: {ex.Message}");
                return;
            }

            // Load MSG with TNEF attachment preservation
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveTnefAttachments = true
            };

            using (MailMessage message = MailMessage.Load(inputMsgPath, loadOptions))
            {
                // Extract all attachments (including those decoded from winmail.dat)
                foreach (Attachment attachment in message.Attachments)
                {
                    string attachmentName = attachment.Name;
                    if (string.IsNullOrEmpty(attachmentName))
                    {
                        attachmentName = "attachment.bin";
                    }

                    string outputPath = Path.Combine(outputDir, attachmentName);
                    try
                    {
                        using (FileStream fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            attachment.Save(fs);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{attachmentName}': {ex.Message}");
                        // Continue with next attachment
                    }
                }

                // Example: add a new text attachment
                string newContent = "This is a new attachment added programmatically.";
                ContentType txtContentType = new ContentType("text/plain");
                Attachment newAttachment = Attachment.CreateAttachmentFromString(newContent, txtContentType);
                newAttachment.Name = "newAttachment.txt";
                message.Attachments.Add(newAttachment);

                // Save the modified message
                try
                {
                    message.Save(modifiedMsgPath, SaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving modified MSG: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
