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
            // Define input and output MSG file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    // Create a simple placeholder MAPI message
                    MapiMessage placeholderMessage = new MapiMessage(
                        "Placeholder Subject",
                        "This is a placeholder message body.",
                        "sender@example.com",
                        "receiver@example.com"
                    );

                    // Save the placeholder to the expected input path
                    placeholderMessage.Save(inputMsgPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the original MSG file and duplicate it
            try
            {
                using (MapiMessage originalMessage = MapiMessage.Load(inputMsgPath))
                {
                    // Clone the message to ensure a separate instance
                    using (MapiMessage duplicatedMessage = originalMessage.Clone())
                    {
                        // Save the duplicated message to the output path
                        duplicatedMessage.Save(outputMsgPath);
                        Console.WriteLine($"Message duplicated successfully to '{outputMsgPath}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG files: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
