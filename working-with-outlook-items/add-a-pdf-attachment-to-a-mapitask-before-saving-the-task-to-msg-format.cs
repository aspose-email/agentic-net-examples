using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pdfPath = "sample.pdf";
            string taskPath = "task.msg";

            // Ensure the PDF file exists; create a minimal placeholder if missing
            if (!File.Exists(pdfPath))
            {
                try
                {
                    byte[] placeholderPdf = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4\n%âãÏÓ\n");
                    File.WriteAllBytes(pdfPath, placeholderPdf);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PDF: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string taskDirectory = Path.GetDirectoryName(taskPath);
            if (!string.IsNullOrEmpty(taskDirectory) && !Directory.Exists(taskDirectory))
            {
                try
                {
                    Directory.CreateDirectory(taskDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create a new MapiTask
            using (MapiTask task = new MapiTask("Sample Task", "Task body", DateTime.Now, DateTime.Now.AddDays(2)))
            {
                // Read PDF data
                byte[] pdfData;
                try
                {
                    pdfData = File.ReadAllBytes(pdfPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read PDF file: {ex.Message}");
                    return;
                }

                // Add PDF attachment to the task
                task.Attachments.Add("sample.pdf", pdfData);

                // Save the task as MSG
                try
                {
                    task.Save(taskPath, TaskSaveFormat.Msg);
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
