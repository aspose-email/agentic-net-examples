using Aspose.Email;
using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Olm;
using Aspose.Email.Mapi;

namespace OlmAttachmentReport
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFilePath = "sample.olm";
                string outputCsvPath = "attachments_report.csv";

                // Verify input file exists
                if (!File.Exists(inputFilePath))
                {
                    Console.Error.WriteLine($"Input OLM file not found: {inputFilePath}");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputCsvPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Open OLM storage
                using (OlmStorage olm = OlmStorage.FromFile(inputFilePath))
                {
                    // Prepare CSV writer
                    using (StreamWriter writer = new StreamWriter(outputCsvPath, false, System.Text.Encoding.UTF8))
                    {
                        // Write CSV header
                        writer.WriteLine("Folder,Subject,AttachmentName");

                        // Iterate folders
                        foreach (OlmFolder folder in olm.GetFolders())
                        {
                            // Iterate messages in folder
                            foreach (OlmMessageInfo messageInfo in folder.EnumerateMessages())
                            {
                                // Extract full message
                                using (MapiMessage message = olm.ExtractMapiMessage(messageInfo))
                                {
                                    // If message has attachments
                                    if (message.Attachments != null && message.Attachments.Count > 0)
                                    {
                                        foreach (MapiAttachment attachment in message.Attachments)
                                        {
                                            // Escape CSV fields
                                            string folderNameEscaped = EscapeCsv(folder.Name);
                                            string subjectEscaped = EscapeCsv(message.Subject);
                                            string attachmentNameEscaped = EscapeCsv(attachment.DisplayName);
                                            writer.WriteLine($"{folderNameEscaped},{subjectEscaped},{attachmentNameEscaped}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }

        private static string EscapeCsv(string field)
        {
            if (field == null)
                return "";
            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                string escaped = field.Replace("\"", "\"\"");
                return $"\"{escaped}\"";
            }
            return field;
        }
    }
}
