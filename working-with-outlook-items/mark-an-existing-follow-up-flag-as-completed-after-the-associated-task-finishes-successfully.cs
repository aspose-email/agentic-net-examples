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
            string msgPath = "flaggedMessage.msg";

            // Ensure the message file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Task with follow‑up flag",
                        "Please complete this task."))
                    {
                        // Set an initial follow‑up flag.
                        FollowUpManager.SetFlag(placeholder, "Follow up");
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder message created at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the existing message, perform a dummy task, then mark the flag as completed.
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Simulate task execution.
                    Console.WriteLine("Performing associated task...");
                    // (Insert real task logic here.)

                    // Mark the follow‑up flag as completed.
                    FollowUpManager.MarkAsCompleted(message);
                    Console.WriteLine("Follow‑up flag marked as completed.");

                    // Save the updated message back to the same file.
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing message file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
