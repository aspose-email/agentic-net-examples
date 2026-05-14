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
            // Prepare output directory and file path
            string outputDirectory = "Output";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputPath = Path.Combine(outputDirectory, "FollowUpMessage.msg");

            // Create a MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Follow‑up Example",
                "Please review the attached information."))
            {
                // Add a custom category to the message
                FollowUpManager.AddCategory(message, "MyCustomCategory");

                // Set a follow‑up flag with a request text
                FollowUpManager.SetFlag(message, "Please follow up");

                // Save the message to MSG format
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Message saved to: {outputPath}");
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
        }
    }
}
