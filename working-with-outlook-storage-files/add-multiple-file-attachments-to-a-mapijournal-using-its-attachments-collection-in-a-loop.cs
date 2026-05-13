using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Output file for the journal
                string outputPath = "journal.msg";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create a new MapiJournal instance
                using (MapiJournal journal = new MapiJournal())
                {
                    journal.Subject = "Project Meeting";
                    journal.Body = "Details of the meeting.";
                    journal.StartTime = DateTime.Now;
                    journal.EndTime = DateTime.Now.AddHours(1);

                    // Files to be attached
                    List<string> attachmentFiles = new List<string>
                    {
                        "file1.txt",
                        "file2.pdf",
                        "image.jpg"
                    };

                    // Add each file as an attachment
                    foreach (string filePath in attachmentFiles)
                    {
                        if (!File.Exists(filePath))
                        {
                            Console.Error.WriteLine($"Attachment file not found: {filePath}");
                            continue;
                        }

                        try
                        {
                            byte[] fileData = File.ReadAllBytes(filePath);
                            string fileName = Path.GetFileName(filePath);
                            journal.Attachments.Add(fileName, fileData);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to attach file '{filePath}': {ex.Message}");
                        }
                    }

                    // Save the journal to a file
                    try
                    {
                        journal.Save(outputPath);
                        Console.WriteLine($"Journal saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save journal: {ex.Message}");
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
