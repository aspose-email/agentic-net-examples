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
            // Define output directory and file path
            string outputDirectory = "Output";
            string messagePath = Path.Combine(outputDirectory, "FollowUpMessage.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MAPI message (sender, recipient, subject, body)
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Action Required",
                "Please review the attached information."))
            {
                // Add a voting button named "Proceed"
                FollowUpManager.AddVotingButton(message, "Proceed");

                // Set a follow‑up flag with a two‑day due date
                DateTime startDate = DateTime.Now;
                DateTime dueDate = startDate.AddDays(2);
                FollowUpManager.SetFlag(message, "Please respond", startDate, dueDate);

                // Save the message to a file
                try
                {
                    message.Save(messagePath);
                    Console.WriteLine($"Message saved to: {messagePath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
