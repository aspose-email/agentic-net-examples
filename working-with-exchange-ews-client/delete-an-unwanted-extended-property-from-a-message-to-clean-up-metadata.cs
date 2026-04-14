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
            // Path to the Outlook MSG file.
            string messagePath = "message.msg";

            // Ensure the file exists. If not, create a minimal placeholder MSG file.
            if (!File.Exists(messagePath))
            {
                try
                {
                    // Create a simple message and save it as a placeholder.
                    MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body.");
                    placeholder.Save(messagePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder message: " + ex.Message);
                    return;
                }
            }

            // Load the existing message.
            using (MapiMessage message = MapiMessage.Load(messagePath))
            {
                // Tag of the unwanted extended property (example tag).
                // Replace this with the actual property tag you need to remove.
                long unwantedPropertyTag = 0x12345678L;

                // Remove the property from the message.
                try
                {
                    message.RemoveProperty(unwantedPropertyTag);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to remove property: " + ex.Message);
                    // Continue; the message will be saved without the removal if it fails.
                }

                // Save the updated message back to the same file.
                try
                {
                    message.Save(messagePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to save updated message: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine("An error occurred: " + ex.Message);
        }
    }
}
