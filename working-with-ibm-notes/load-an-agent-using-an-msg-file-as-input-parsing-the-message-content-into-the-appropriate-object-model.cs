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
            string msgFilePath = "agent.msg";

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgFilePath))
                {
                    Console.WriteLine($"Subject: {msg.Subject}");
                    Console.WriteLine($"From: {msg.SenderName}");
                    Console.WriteLine($"Body: {msg.Body}");

                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        Console.WriteLine($"Attachment Name: {attachment.FileName}");
                        try
                        {
                            attachment.Save(attachment.FileName);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
