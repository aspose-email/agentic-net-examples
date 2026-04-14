using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output directory and file path
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "Draft.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a draft MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Project Update",
                "Please review the attached document and respond by the deadline."))
            {
                // Define the deadline for the follow‑up flag
                DateTime deadline = DateTime.Now.AddDays(3);

                // Assign a follow‑up flag to the recipient with a reminder time
                FollowUpManager.SetFlagForRecipients(message, "Please respond", deadline);

                // Save the draft message to disk
                message.Save(outputPath);
                Console.WriteLine($"Draft message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
