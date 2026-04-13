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
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(messagePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the existing message, clear voting buttons, add a single "Accepted" button, and save.
            try
            {
                using (MapiMessage message = MapiMessage.Load(messagePath))
                {
                    FollowUpManager.ClearVotingButtons(message);
                    FollowUpManager.AddVotingButton(message, "Accepted");
                    message.Save(messagePath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
