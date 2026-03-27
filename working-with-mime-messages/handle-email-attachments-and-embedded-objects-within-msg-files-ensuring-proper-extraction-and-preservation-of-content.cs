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
            string msgPath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file.
            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    Console.WriteLine($"Subject: {msg.Subject}");
                    Console.WriteLine($"From: {msg.SenderName}");

                    // Iterate through attachments and save each one.
                    foreach (MapiAttachment attachment in msg.Attachments)
                    {
                        Console.WriteLine($"Attachment Name: {attachment.FileName}");
                        try
                        {
                            // Save the attachment to the current directory.
                            attachment.Save(attachment.FileName);
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
