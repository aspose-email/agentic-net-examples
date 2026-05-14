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
            // Create a new MAPI message (4‑argument constructor required)
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Project Update",
                "Please review the attached documents."))
            {
                // Add a voting button named "Finalize"
                FollowUpManager.AddVotingButton(message, "Finalize");

                // Calculate the date of the next Monday
                DateTime today = DateTime.Today;
                int daysUntilMonday = ((int)DayOfWeek.Monday - (int)today.DayOfWeek + 7) % 7;
                if (daysUntilMonday == 0)
                {
                    daysUntilMonday = 7; // Ensure we get the *next* Monday, not today if today is Monday
                }
                DateTime nextMonday = today.AddDays(daysUntilMonday);

                // Set a follow‑up flag with a start date of now and a due date of next Monday
                FollowUpManager.SetFlag(message, "Please finalize", DateTime.Now, nextMonday);

                // Define output path
                string outputPath = "output.msg";

                // Ensure the target directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
