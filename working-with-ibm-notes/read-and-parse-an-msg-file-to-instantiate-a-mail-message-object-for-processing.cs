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

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Subject";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(msgFilePath);
                    }
                    Console.WriteLine($"Placeholder MSG file created at '{msgFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Read and parse the MSG file.
            try
            {
                using (MapiMessageReader reader = new MapiMessageReader(msgFilePath))
                {
                    using (MapiMessage message = reader.ReadMessage())
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                        Console.WriteLine("From: " + message.SenderName);
                        Console.WriteLine("Body: " + message.Body);

                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            Console.WriteLine("Attachment: " + attachment.FileName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
