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
            // Define file paths
            string outputMsgPath = Path.Combine(Environment.CurrentDirectory, "output.msg");
            string attachmentPath = Path.Combine(Environment.CurrentDirectory, "sample.txt");
            string attachmentSaveDir = Path.Combine(Environment.CurrentDirectory, "Attachments");

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Ensure attachment directory exists
            if (!Directory.Exists(attachmentSaveDir))
            {
                Directory.CreateDirectory(attachmentSaveDir);
            }

            // Create a placeholder attachment file if it does not exist
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "This is a sample attachment.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Create a new MailMessage
            using (MailMessage mailMessage = new MailMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the body of the message."))
            {
                // Add attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    mailMessage.Attachments.Add(attachment);
                }

                // Save as MSG with preserved dates
                MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };

                try
                {
                    mailMessage.Save(outputMsgPath, msgSaveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the saved MSG file as MapiMessage
            if (!File.Exists(outputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(outputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine("MSG file was not created.");
                return;
            }

            using (MapiMessage loadedMsg = MapiMessage.Load(outputMsgPath))
            {
                // Iterate over headers using Keys as required
                foreach (string key in loadedMsg.Headers.Keys)
                {
                    string value = loadedMsg.Headers[key];
                    Console.WriteLine($"{key}: {value}");
                }

                // Process attachments
                foreach (MapiAttachment att in loadedMsg.Attachments)
                {
                    string attName = !string.IsNullOrEmpty(att.LongFileName) ? att.LongFileName : att.FileName;
                    Console.WriteLine($"Attachment found: {attName}");

                    // Save attachment binary data if present
                    if (att.BinaryData != null && att.BinaryData.Length > 0)
                    {
                        string savePath = Path.Combine(attachmentSaveDir, attName);
                        try
                        {
                            File.WriteAllBytes(savePath, att.BinaryData);
                            Console.WriteLine($"Attachment saved to: {savePath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attName}': {ex.Message}");
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
