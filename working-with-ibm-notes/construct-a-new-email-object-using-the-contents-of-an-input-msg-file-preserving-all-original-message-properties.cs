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
            // Path to the source MSG file
            string inputPath = "input.msg";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load the original message from the MSG file
            using (MapiMessage originalMessage = MapiMessage.Load(inputPath))
            {
                // Clone the message to create a new independent instance
                MapiMessage newMessage = (MapiMessage)originalMessage.Clone();

                // Ensure the cloned message is disposed after use
                using (newMessage)
                {
                    // Path for the new MSG file
                    string outputPath = "output.msg";

                    // Save the new message, preserving all original properties
                    newMessage.Save(outputPath);
                    Console.WriteLine($"New message saved to: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
