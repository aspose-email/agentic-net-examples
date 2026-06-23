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
            // Path to the message file (MSG). Adjust as needed.
            string msgPath = "sample.msg";

            // Ensure the file exists; if not, create a minimal placeholder message.
            if (!File.Exists(msgPath))
            {
                try
                {
                    // Create a simple MAPI message with a voting button for demonstration.
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "Sample body"))
                    {
                        // Add a voting button so we have something to clear later.
                        FollowUpManager.AddVotingButton(placeholder, "Approve");
                        // Save the placeholder message.
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message, mark it as completed, and remove voting buttons.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Mark the message as completed (sets the follow‑up flag status).
                    FollowUpManager.MarkAsCompleted(message);

                    // Remove all voting buttons from the completed message.
                    FollowUpManager.ClearVotingButtons(message);

                    // Save the updated message back to the same file.
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                return;
            }

            Console.WriteLine("Voting buttons removed from completed message successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
