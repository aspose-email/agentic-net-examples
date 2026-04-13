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
            string msgPath = "input.msg";
            string attachmentPath = "attachment.txt";
            string outputPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder message."))
                {
                    try
                    {
                        placeholder.Save(msgPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                        return;
                    }
                }
            }

            // Ensure the attachment file exists; create a tiny placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                // Read attachment bytes
                byte[] attachmentData;
                try
                {
                    attachmentData = File.ReadAllBytes(attachmentPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read attachment file: {ex.Message}");
                    return;
                }

                // Add the attachment to the message
                try
                {
                    message.Attachments.Add(Path.GetFileName(attachmentPath), attachmentData);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    return;
                }

                // Save the updated message
                try
                {
                    message.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
