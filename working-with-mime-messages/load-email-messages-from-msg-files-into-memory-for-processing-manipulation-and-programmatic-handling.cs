using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgLoader
{
    // Author: Example code for loading MSG files using Aspose.Email
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Directory that contains the MSG files
                string messagesFolder = "Messages";

                // Verify the directory exists
                if (!Directory.Exists(messagesFolder))
                {
                    Console.Error.WriteLine($"Directory not found: {messagesFolder}");
                    return;
                }

                // Process each MSG file in the directory
                foreach (string msgFilePath in Directory.GetFiles(messagesFolder, "*.msg"))
                {
                    try
                    {
                        // Guard against missing file (should not happen after GetFiles)
                        if (!File.Exists(msgFilePath))
                        {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                            Console.Error.WriteLine($"File not found: {msgFilePath}");
                            continue;
                        }

                        // Load the Outlook message
                        MapiMessage msg = MapiMessage.Load(msgFilePath);

                        // Display basic properties
                        Console.WriteLine($"Loaded: {Path.GetFileName(msgFilePath)}");
                        Console.WriteLine($"Subject: {msg.Subject}");
                        Console.WriteLine($"From: {msg.SenderName}");
                        Console.WriteLine($"Body: {msg.Body}");

                        // List attachments and optionally save them
                        foreach (MapiAttachment attachment in msg.Attachments)
                        {
                            Console.WriteLine($"Attachment: {attachment.FileName}");
                            // Save attachment to the same folder (overwrite if exists)
                            string attachmentPath = Path.Combine(messagesFolder, attachment.FileName);
                            attachment.Save(attachmentPath);
                        }

                        // Example manipulation: prepend a tag to the subject
                        msg.Subject = $"[Processed] {msg.Subject}";

                        // Save the modified message to a new file
                        string processedPath = Path.Combine(messagesFolder, $"Processed_{Path.GetFileName(msgFilePath)}");
                        msg.Save(processedPath);
                        Console.WriteLine($"Saved modified message to: {processedPath}");
                        Console.WriteLine();
                    }
                    catch (Exception exFile)
                    {
                        Console.Error.WriteLine($"Error processing file '{msgFilePath}': {exFile.Message}");
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
