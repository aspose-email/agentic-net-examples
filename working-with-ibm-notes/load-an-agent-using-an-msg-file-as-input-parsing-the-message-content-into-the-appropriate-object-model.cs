using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "sample.msg";

                if (!File.Exists(msgFilePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                    return;
                }

                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("From: " + message.SenderEmailAddress);
                    Console.WriteLine("Body: " + message.Body);

                    if (message.Attachments != null && message.Attachments.Count > 0)
                    {
                        string attachmentDir = "Attachments";
                        try
                        {
                            if (!Directory.Exists(attachmentDir))
                            {
                                Directory.CreateDirectory(attachmentDir);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating attachment directory: {ex.Message}");
                            return;
                        }

                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            string attachmentPath = Path.Combine(attachmentDir, attachment.FileName);
                            try
                            {
                                attachment.Save(attachmentPath);
                                Console.WriteLine($"Saved attachment: {attachmentPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error saving attachment {attachment.FileName}: {ex.Message}");
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
}
