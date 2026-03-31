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

            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage instance
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Display basic properties
                Console.WriteLine($"Subject: {message.Subject}");
                Console.WriteLine($"From: {message.SenderName} <{message.SenderEmailAddress}>");
                Console.WriteLine($"Body: {message.Body}");

                // Iterate attachments if any
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    Console.WriteLine($"Attachment: {attachment.FileName} ({attachment.BinaryData?.Length ?? 0} bytes)");
                    // Save attachment to disk
                    string attachmentPath = Path.Combine("Attachments", attachment.FileName);
                    try
                    {
                        string attachmentDir = Path.GetDirectoryName(attachmentPath);
                        if (!Directory.Exists(attachmentDir))
                        {
                            Directory.CreateDirectory(attachmentDir);
                        }
                        File.WriteAllBytes(attachmentPath, attachment.BinaryData);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving attachment '{attachment.FileName}': {ex.Message}");
                    }
                }

                // Example manipulation: change subject
                message.Subject = "Updated Subject";

                // Save modified message to a new file
                string outputPath = "modified.msg";
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Modified message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving modified message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
