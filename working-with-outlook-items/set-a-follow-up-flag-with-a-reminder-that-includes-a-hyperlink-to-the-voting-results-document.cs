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
            // Define output file path for the draft message
            string outputPath = "DraftMessage.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a draft MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Follow‑up Request",
                "Please review the voting results: https://example.com/vote-results.docx"))
            {
                // Set a follow‑up flag for recipients with a reminder time
                DateTime reminderTime = DateTime.Now.AddHours(2);
                FollowUpManager.SetFlagForRecipients(message,
                    "Please review the voting results: https://example.com/vote-results.docx",
                    reminderTime);

                // Save the draft message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Draft message saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save the message: {ioEx.Message}");
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
