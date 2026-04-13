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
            string messagePath = "message.msg";

            // Ensure the message file exists; create a minimal placeholder if missing.
            if (!File.Exists(messagePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the message, mark as completed, and remove voting buttons.
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    // Mark the message as completed (if not already).
                    FollowUpManager.MarkAsCompleted(message);

                    // Remove any voting buttons present.
                    FollowUpManager.ClearVotingButtons(message);

                    // Save the updated message back to the file.
                    message.Save(messagePath);
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
