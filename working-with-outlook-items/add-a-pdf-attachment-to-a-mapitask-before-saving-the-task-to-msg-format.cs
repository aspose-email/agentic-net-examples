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
            // Paths for the PDF attachment and the output MSG file
            string pdfPath = "sample.pdf";
            string msgPath = "task_with_attachment.msg";

            // Verify that the PDF file exists
            if (!File.Exists(pdfPath))
            {
                Console.Error.WriteLine($"PDF file not found: {pdfPath}");
                return;
            }

            // Read the PDF data
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

            // Create a MapiTask and add the PDF attachment
            using (MapiTask task = new MapiTask("Task Subject", "Task Body", DateTime.Now, DateTime.Now.AddDays(2)))
            {
                // Add attachment using the collection's Add method (name, data)
                task.Attachments.Add(Path.GetFileName(pdfPath), pdfData);

                // Save the task as an MSG file
                try
                {
                    task.Save(msgPath, TaskSaveFormat.Msg);
                    Console.WriteLine($"Task saved with attachment to: {msgPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
