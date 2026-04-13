using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AttachmentInventory
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input folder containing MSG files
                string inputFolder = "MsgFiles";
                // Output report file path
                string reportPath = "AttachmentReport.txt";

                // Verify input folder exists
                if (!Directory.Exists(inputFolder))
                {
                    Console.Error.WriteLine($"Error: Input folder not found – {inputFolder}");
                    return;
                }

                // Prepare a dictionary to aggregate attachment information
                // Key: attachment file name, Value: occurrence count
                Dictionary<string, int> attachmentInventory = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

                // Process each MSG file in the folder
                string[] msgFiles;
                try
                {
                    msgFiles = Directory.GetFiles(inputFolder, "*.msg");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing files in folder – {ex.Message}");
                    return;
                }

                foreach (string filePath in msgFiles)
                {
                    // Guard against missing file (should not happen after GetFiles, but safe)
                    if (!File.Exists(filePath))
                    {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"Warning: File not found – {filePath}");
                        continue;
                    }

                    try
                    {
                        // Load the MSG file
                        using (MapiMessage message = MapiMessage.Load(filePath))
                        {
                            // Iterate through attachments
                            foreach (MapiAttachment attachment in message.Attachments)
                            {
                                string attachmentName = attachment.FileName ?? "UnnamedAttachment";

                                if (attachmentInventory.ContainsKey(attachmentName))
                                {
                                    attachmentInventory[attachmentName] = attachmentInventory[attachmentName] + 1;
                                }
                                else
                                {
                                    attachmentInventory.Add(attachmentName, 1);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to process '{filePath}': {ex.Message}");
                        // Continue with next file
                    }
                }

                // Prepare report lines
                List<string> reportLines = new List<string>();
                reportLines.Add("Attachment Inventory Report");
                reportLines.Add($"Generated on: {DateTime.Now}");
                reportLines.Add(string.Empty);
                reportLines.Add("Attachment Name,Occurrences");
                foreach (KeyValuePair<string, int> entry in attachmentInventory)
                {
                    reportLines.Add($"{entry.Key},{entry.Value}");
                }

                // Ensure the directory for the report exists
                string reportDirectory = Path.GetDirectoryName(reportPath);
                if (!string.IsNullOrEmpty(reportDirectory) && !Directory.Exists(reportDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(reportDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error creating report directory – {ex.Message}");
                        return;
                    }
                }

                // Write the report to file
                try
                {
                    File.WriteAllLines(reportPath, reportLines);
                    Console.WriteLine($"Attachment inventory report written to '{reportPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error writing report file – {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
