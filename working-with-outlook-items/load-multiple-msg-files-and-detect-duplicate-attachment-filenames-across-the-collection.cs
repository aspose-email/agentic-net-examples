using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace DuplicateAttachmentDetector
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Directory containing MSG files
                string folderPath = "Messages";

                // Verify the directory exists
                if (!Directory.Exists(folderPath))
                {
                    Console.Error.WriteLine($"Error: Directory not found – {folderPath}");
                    return;
                }

                // Get all MSG files in the directory
                string[] msgPaths = Directory.GetFiles(folderPath, "*.msg");
                if (msgPaths.Length == 0)
                {
                    Console.Error.WriteLine($"Error: No MSG files found in – {folderPath}");
                    return;
                }

                // Dictionary to count attachment filenames (case‑insensitive)
                Dictionary<string, int> attachmentCounts = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Process each MSG file
                foreach (string msgPath in msgPaths)
                {
                    // Guard against missing files (should not happen after GetFiles, but safe)
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

                        Console.Error.WriteLine($"Warning: File not found – {msgPath}");
                        continue;
                    }

                    try
                    {
                        // Load the MSG file
                        using (MapiMessage message = MapiMessage.Load(msgPath))
                        {
                            // Iterate over attachments
                            foreach (MapiAttachment attachment in message.Attachments)
                            {
                                string fileName = attachment.FileName;
                                if (attachmentCounts.ContainsKey(fileName))
                                {
                                    attachmentCounts[fileName] += 1;
                                }
                                else
                                {
                                    attachmentCounts.Add(fileName, 1);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing file {msgPath}: {ex.Message}");
                    }
                }

                // Report duplicate attachment filenames
                bool duplicatesFound = false;
                foreach (KeyValuePair<string, int> kvp in attachmentCounts)
                {
                    if (kvp.Value > 1)
                    {
                        duplicatesFound = true;
                        Console.WriteLine($"Duplicate attachment filename: {kvp.Key} appears {kvp.Value} times.");
                    }
                }

                if (!duplicatesFound)
                {
                    Console.WriteLine("No duplicate attachment filenames found.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
