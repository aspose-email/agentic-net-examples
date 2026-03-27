using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

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

            try
            {
                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("From: " + message.SenderName);
                    Console.WriteLine("Body: " + message.Body);

                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        Console.WriteLine("Attachment Name: " + attachment.FileName);
                        string attachmentPath = Path.Combine(Environment.CurrentDirectory, attachment.FileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                            Console.WriteLine("Saved attachment to: " + attachmentPath);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
