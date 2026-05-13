using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define attachment file paths
            string[] attachmentPaths = { "file1.txt", "file2.pdf" };

            // Ensure each attachment file exists; create a minimal placeholder if missing
            foreach (string path in attachmentPaths)
            {
                if (!File.Exists(path))
                {
                    try
                    {
                        File.WriteAllText(path, "Placeholder content");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder file '{path}': {ex.Message}");
                        return;
                    }
                }
            }

            // Create a new task
            using (Aspose.Email.Calendar.Task task = new Aspose.Email.Calendar.Task())
            {
                task.Subject = "Sample Task";
                task.Body = "Aspose.Email.Calendar.Task with multiple attachments";

                // Add each attachment to the task
                foreach (string path in attachmentPaths)
                {
                    try
                    {
                        Attachment attachment = new Attachment(path);
                        task.Attachments.Add(attachment);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachment '{path}': {ex.Message}");
                        return;
                    }
                }

                // Verify attachment count
                int attachmentCount = task.Attachments.Count;
                Console.WriteLine($"Attachment count: {attachmentCount}");

                // Prepare output path
                string outputPath = "TaskWithAttachments.msg";
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {ex.Message}");
                        return;
                    }
                }

                // Save the task to a MSG file
                try
                {
                    task.Save(outputPath);
                    Console.WriteLine($"Aspose.Email.Calendar.Task saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
