using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailMsgReader
{
    // Author: Generated example for loading MSG files with Aspose.Email
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Determine the MSG file path (use first argument or default)
                string msgPath = args.Length > 0 ? args[0] : "input.msg";

                // Guard file existence
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

                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Load the Outlook MSG file
                MapiMessage msg = MapiMessage.Load(msgPath);

                // Display basic properties
                Console.WriteLine("Subject: " + msg.Subject);
                Console.WriteLine("From: " + msg.SenderName);
                Console.WriteLine("Body: " + msg.Body);

                // Process attachments if any
                foreach (MapiAttachment attachment in msg.Attachments)
                {
                    Console.WriteLine("Attachment Name: " + attachment.FileName);

                    // Save attachment to the same directory as the MSG file
                    try
                    {
                        string outputDir = Path.GetDirectoryName(msgPath) ?? Directory.GetCurrentDirectory();
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        string attachmentPath = Path.Combine(outputDir, attachment.FileName);
                        attachment.Save(attachmentPath);
                        Console.WriteLine($"Saved attachment to: {attachmentPath}");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ioEx.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
