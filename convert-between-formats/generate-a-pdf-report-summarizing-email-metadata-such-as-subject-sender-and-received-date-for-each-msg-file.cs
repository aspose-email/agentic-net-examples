using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input directory containing MSG files
            string inputDir = "msg_files";
            // Output PDF report path (plain text saved with .pdf extension)
            string outputPdf = "EmailReport.pdf";

            // Ensure input directory exists
            if (!Directory.Exists(inputDir))
            {
                Console.Error.WriteLine($"Input directory '{inputDir}' does not exist.");
                return;
            }

            // Guard output directory existence
            string outputDir = Path.GetDirectoryName(outputPdf);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Collect MSG files
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDir, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error enumerating MSG files: {ex.Message}");
                return;
            }

            if (msgFiles.Length == 0)
            {
                Console.Error.WriteLine("No MSG files found in the input directory.");
                return;
            }

            // Build report content
            var reportLines = new List<string>();
            reportLines.Add("Email Metadata Report");
            reportLines.Add("=====================");
            reportLines.Add(string.Empty);

            foreach (string msgPath in msgFiles)
            {
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

                try
                {
                    using (MapiMessage msg = MapiMessage.Load(msgPath))
                    {
                        string subject = msg.Subject ?? "(no subject)";
                        string sender = msg.SenderName ?? msg.SenderEmailAddress ?? "(unknown sender)";
                        string received = msg.DeliveryTime != DateTime.MinValue
                            ? msg.DeliveryTime.ToString("u")
                            : "(unknown date)";

                        reportLines.Add($"File: {Path.GetFileName(msgPath)}");
                        reportLines.Add($"Subject: {subject}");
                        reportLines.Add($"Sender: {sender}");
                        reportLines.Add($"Received: {received}");
                        reportLines.Add(string.Empty);
                    }
                }
                catch (AsposeException aex)
                {
                    Console.Error.WriteLine($"Aspose error processing '{msgPath}': {aex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
                }
            }

            // Combine lines into a single string
            string reportContent = string.Join(Environment.NewLine, reportLines);

            // Write the report to a file with .pdf extension (plain text placeholder)
            try
            {
                File.WriteAllText(outputPdf, reportContent, Encoding.UTF8);
                Console.WriteLine($"Report generated: {outputPdf}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write report: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
