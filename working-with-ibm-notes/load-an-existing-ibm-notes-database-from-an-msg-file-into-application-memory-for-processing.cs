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

            using (MapiMessage message = MapiMessage.Load(msgFilePath))
            {
                // Display basic message information
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.SenderEmailAddress}");
                Console.WriteLine($"Body: {message.Body}");

                // List attachments, if any
                foreach (var attachment in message.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
