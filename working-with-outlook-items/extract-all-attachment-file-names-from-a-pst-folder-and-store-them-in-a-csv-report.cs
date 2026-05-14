using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace ExtractPstAttachments
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input PST file path
                string pstFilePath = "sample.pst";

                // Output CSV report path
                string csvReportPath = "attachments_report.csv";

                // Verify PST file exists
                if (!File.Exists(pstFilePath))
                {
                    Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                    return;
                }

                // Ensure the directory for the CSV report exists
                string csvDirectory = Path.GetDirectoryName(csvReportPath);
                if (!string.IsNullOrEmpty(csvDirectory) && !Directory.Exists(csvDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(csvDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create directory for CSV report: {dirEx.Message}");
                        return;
                    }
                }

                // List to hold CSV rows
                List<string> csvLines = new List<string>();
                // Add CSV header
                csvLines.Add("MessageSubject,AttachmentFileName");

                // Open PST file
                using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
                {
                    // Specify the folder to process; using root folder in this example
                    FolderInfo targetFolder = pst.RootFolder;

                    // Iterate through each message in the folder
                    foreach (MessageInfo messageInfo in targetFolder.EnumerateMessages())
                    {
                        // Extract attachments for the current message
                        MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);

                        // If there are no attachments, continue to next message
                        if (attachments == null || attachments.Count == 0)
                        {
                            continue;
                        }

                        // Record each attachment file name with the message subject
                        foreach (MapiAttachment attachment in attachments)
                        {
                            string sanitizedSubject = messageInfo.Subject?.Replace("\"", "\"\"") ?? string.Empty;
                            string sanitizedFileName = attachment.FileName?.Replace("\"", "\"\"") ?? string.Empty;
                            // Enclose fields in quotes to handle commas
                            string csvLine = $"\"{sanitizedSubject}\",\"{sanitizedFileName}\"";
                            csvLines.Add(csvLine);
                        }
                    }
                }

                // Write CSV report to file
                try
                {
                    using (StreamWriter writer = new StreamWriter(csvReportPath, false))
                    {
                        foreach (string line in csvLines)
                        {
                            writer.WriteLine(line);
                        }
                    }

                    Console.WriteLine($"Attachment report generated: {csvReportPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write CSV report: {writeEx.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
