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
            // Define output path for the draft message
            string outputPath = "DraftMessage.msg";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a draft MAPI message (must use the 4‑argument constructor)
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Project Update Required",
                "Please review the attached document and respond by the due date."))
            {
                // Define the reminder deadline (e.g., 3 days from now)
                DateTime reminderTime = DateTime.Now.AddDays(3);

                // Assign a follow‑up flag for the recipient with a reminder
                FollowUpManager.SetFlagForRecipients(message, "Please respond", reminderTime);

                // Save the draft message to disk
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
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
