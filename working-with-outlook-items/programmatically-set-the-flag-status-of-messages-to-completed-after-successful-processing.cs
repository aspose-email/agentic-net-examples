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
            // Path to the Outlook MSG file
            string msgPath = "message.msg";

            // Guard against missing file
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the message, process it, and mark as completed
            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                // Placeholder for processing logic
                Console.WriteLine($"Processing message: {message.Subject}");

                // Mark the flagged message as completed
                FollowUpManager.MarkAsCompleted(message);

                // Save the updated message back to the same file
                message.Save(msgPath);
                Console.WriteLine("Message flagged as Completed and saved.");
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
