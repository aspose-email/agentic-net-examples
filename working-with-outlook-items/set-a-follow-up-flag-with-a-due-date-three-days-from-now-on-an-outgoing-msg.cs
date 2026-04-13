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
            // Define output MSG file path
            string outputPath = "output.msg";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Test Subject", "This is the body of the message."))
            {
                // Set follow‑up flag with start date now and due date three days from now
                DateTime startDate = DateTime.Now;
                DateTime dueDate = startDate.AddDays(3);
                string flagRequest = "Follow up";

                FollowUpManager.SetFlag(message, flagRequest, startDate, dueDate);

                // Save the message as MSG
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
