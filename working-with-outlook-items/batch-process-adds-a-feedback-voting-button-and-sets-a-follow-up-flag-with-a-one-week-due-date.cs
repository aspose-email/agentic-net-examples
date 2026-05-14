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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message, modify, and save.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Add a voting button named "Feedback".
                    FollowUpManager.AddVotingButton(message, "Feedback");

                    // Set a follow‑up flag with a one‑week due date.
                    DateTime startDate = DateTime.Now;
                    DateTime dueDate = startDate.AddDays(7);
                    FollowUpManager.SetFlag(message, "Please provide feedback", startDate, dueDate);

                    // Save the modified message.
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
