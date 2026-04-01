using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output MSG file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";
            string targetAttachmentName = "target.txt";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    // Create a simple placeholder message with a dummy attachment
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Body = "This is a placeholder MSG file.";
                        // Add a dummy attachment so the file is a valid MSG with attachments
                        using (Attachment dummy = new Attachment("dummy.txt"))
                        {
                            placeholder.Attachments.Add(dummy);
                        }

                        // Ensure the directory for the input path exists
                        string inputDir = Path.GetDirectoryName(inputMsgPath);
                        if (!string.IsNullOrEmpty(inputDir) && !Directory.Exists(inputDir))
                        {
                            Directory.CreateDirectory(inputDir);
                        }

                        placeholder.Save(inputMsgPath, SaveOptions.DefaultMsgUnicode);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            using (MailMessage message = MailMessage.Load(inputMsgPath))
            {
                // Find the attachment to remove
                Attachment attachmentToRemove = null;
                foreach (Attachment attachment in message.Attachments)
                {
                    if (string.Equals(attachment.Name, targetAttachmentName, StringComparison.OrdinalIgnoreCase))
                    {
                        attachmentToRemove = attachment;
                        break;
                    }
                }

                // Remove the targeted attachment if found
                if (attachmentToRemove != null)
                {
                    message.Attachments.Remove(attachmentToRemove);
                    Console.WriteLine($"Removed attachment: {targetAttachmentName}");
                }
                else
                {
                    Console.WriteLine($"Attachment '{targetAttachmentName}' not found.");
                }

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputMsgPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the modified MSG file
                try
                {
                    message.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Modified MSG saved to: {outputMsgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save modified MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
