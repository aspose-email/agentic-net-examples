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
            // Path to the MSG file supplied by the user (can be changed as needed)
            string msgFilePath = "input.msg";

            // Ensure the file exists; if not, create a minimal placeholder MSG file
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    // Create a simple placeholder message
                    MapiMessage placeholderMessage = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "This is a placeholder MSG file created because the original file was missing."
                    );

                    // Save the placeholder MSG using Unicode format
                    placeholderMessage.Save(msgFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file safely
            MapiMessage loadedMessage;
            try
            {
                loadedMessage = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Process the loaded message (display basic information)
            using (loadedMessage)
            {
                Console.WriteLine($"Subject: {loadedMessage.Subject}");
                Console.WriteLine($"From: {loadedMessage.SenderName} <{loadedMessage.SenderEmailAddress}>");
                Console.WriteLine($"Body: {loadedMessage.Body}");

                // List attachments, if any
                if (loadedMessage.Attachments != null && loadedMessage.Attachments.Count > 0)
                {
                    Console.WriteLine("Attachments:");
                    foreach (MapiAttachment attachment in loadedMessage.Attachments)
                    {
                        Console.WriteLine($"- {attachment.FileName}");
                        // Optionally, save each attachment to disk (guarded)
                        string attachmentPath = Path.Combine(Path.GetDirectoryName(msgFilePath) ?? "", attachment.FileName);
                        try
                        {
                            attachment.Save(attachmentPath);
                            Console.WriteLine($"  Saved to: {attachmentPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"  Error saving attachment '{attachment.FileName}': {ex.Message}");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No attachments found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
