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
            string inputPath = "sample.msg";
            string outputPath = "sample_updated.msg";

            // Ensure the input file exists; if not, create a minimal placeholder MSG.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder", "This is a placeholder message."))
                    {
                        placeholder.Save(inputPath);
                        Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file, mark follow‑up as completed, and save.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Mark the follow‑up flag as completed.
                    FollowUpManager.MarkAsCompleted(message);

                    // Save the updated message.
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with completed follow‑up flag to '{outputPath}'.");
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
