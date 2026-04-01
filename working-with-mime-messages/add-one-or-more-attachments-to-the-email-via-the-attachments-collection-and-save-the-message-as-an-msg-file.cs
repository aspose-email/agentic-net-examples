using System;
using System.IO;
using Aspose.Email;

namespace AddAttachmentsExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Paths for the output MSG file and attachments
                string outputMsgPath = "AddAttachments.msg";
                string attachmentPath1 = "sample.txt";
                string attachmentPath2 = "image.jpg";

                // Ensure attachment files exist; create minimal placeholders if they do not
                try
                {
                    if (!File.Exists(attachmentPath1))
                    {
                        File.WriteAllText(attachmentPath1, "Placeholder text content.");
                    }

                    if (!File.Exists(attachmentPath2))
                    {
                        using (FileStream fs = File.Create(attachmentPath2))
                        {
                            // Create an empty placeholder image file
                        }
                    }
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to prepare attachment files: {ioEx.Message}");
                    return;
                }

                // Ensure the directory for the output file exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(outputMsgPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Create the email message and add attachments
                using (MailMessage mailMessage = new MailMessage())
                {
                    mailMessage.From = "sender@example.com";
                    mailMessage.To.Add("receiver@example.com");
                    mailMessage.Subject = "Message with attachments";
                    mailMessage.Body = "Please find the attachments.";

                    // Load attachments and add them to the message
                    using (Attachment attachment1 = new Attachment(attachmentPath1))
                    using (Attachment attachment2 = new Attachment(attachmentPath2))
                    {
                        mailMessage.Attachments.Add(attachment1);
                        mailMessage.AddAttachment(attachment2);
                    }

                    // Save the message as an Outlook MSG file
                    try
                    {
                        mailMessage.Save(outputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save MSG file: {saveEx.Message}");
                        return;
                    }
                }

                Console.WriteLine("Message saved successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
