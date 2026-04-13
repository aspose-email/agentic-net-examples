using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Mapi;

namespace SuspiciousAttachmentDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Folder that contains Outlook MSG files
                string messagesFolder = "Messages";

                // Verify the folder exists
                if (!Directory.Exists(messagesFolder))
                {
                    Console.Error.WriteLine($"Folder not found: {messagesFolder}");
                    return;
                }

                // Define a whitelist of allowed attachment extensions (case‑insensitive)
                HashSet<string> allowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
                {
                    ".pdf",
                    ".docx",
                    ".xlsx",
                    ".txt"
                };

                // Get all MSG files in the folder
                string[] msgFiles;
                try
                {
                    msgFiles = Directory.GetFiles(messagesFolder, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                    return;
                }

                foreach (string msgPath in msgFiles)
                {
                    // Ensure the file still exists before processing
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

                        Console.Error.WriteLine($"File not found: {msgPath}");
                        continue;
                    }

                    // Load the MSG file inside a using block to ensure disposal
                    try
                    {
                        using (MapiMessage message = MapiMessage.Load(msgPath))
                        {
                            bool hasSuspiciousAttachment = false;

                            foreach (MapiAttachment attachment in message.Attachments)
                            {
                                string extension = Path.GetExtension(attachment.FileName);
                                if (!allowedExtensions.Contains(extension))
                                {
                                    hasSuspiciousAttachment = true;
                                    Console.WriteLine($"Suspicious attachment detected in '{Path.GetFileName(msgPath)}': {attachment.FileName}");
                                }
                            }

                            if (!hasSuspiciousAttachment)
                            {
                                Console.WriteLine($"No suspicious attachments in '{Path.GetFileName(msgPath)}'.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
                        // Continue with next file
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
