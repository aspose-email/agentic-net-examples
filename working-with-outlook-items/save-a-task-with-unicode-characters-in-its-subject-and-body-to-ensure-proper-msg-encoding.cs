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
            // Define the output MSG file path
            string outputPath = "task_unicode.msg";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a task message with Unicode characters in subject and body
            using (MapiMessage taskMessage = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Subject with Unicode 🚀",
                "Body with Unicode 漢字",
                OutlookMessageFormat.Unicode))
            {
                // Set the message class to Task
                taskMessage.MessageClass = "IPM.Task";

                // Save the message as MSG (Unicode encoding)
                taskMessage.Save(outputPath);
                Console.WriteLine($"Task message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
