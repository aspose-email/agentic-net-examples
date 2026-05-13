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
            // Path to the MSG file that contains voting buttons
            string msgPath = "message.msg";

            // Ensure the file exists; if not, create a minimal placeholder MSG
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Sample Message",
                        "This is a placeholder message with no voting buttons."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the message, clear voting buttons, and save the changes
            try
            {
                using (MapiMessage message = MapiMessage.Load(msgPath))
                {
                    // Delete any voting buttons that may be present
                    FollowUpManager.ClearVotingButtons(message);

                    // Save the updated message back to the same file
                    message.Save(msgPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
