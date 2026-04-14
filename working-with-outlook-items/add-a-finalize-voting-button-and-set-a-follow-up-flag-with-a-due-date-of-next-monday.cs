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
            // Define output file path
            string outputPath = "FinalizeMessage.msg";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Please review",
                "Kindly review the attached document."))
            {
                // Add a voting button named "Finalize"
                FollowUpManager.AddVotingButton(message, "Finalize");

                // Calculate next Monday's date
                DateTime today = DateTime.Today;
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                daysUntilMonday = daysUntilMonday == 0 ? 7 : daysUntilMonday; // ensure next Monday, not today
                DateTime nextMonday = today.AddDays(daysUntilMonday);

                // Set a follow‑up flag with due date of next Monday
                FollowUpManager.SetFlag(message, "Please finalize", DateTime.Now, nextMonday);

                // Save the message to a .msg file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
