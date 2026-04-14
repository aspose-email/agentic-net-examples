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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "from@example.com",
                    "to@example.com",
                    "Placeholder Subject",
                    "Placeholder Body"))
                {
                    placeholder.Save(inputPath);
                    Console.WriteLine($"Created placeholder MSG at {inputPath}");
                }
            }

            // Load the MSG with PreserveEmbeddedMessageFormat set to true
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveEmbeddedMessageFormat = true
            };

            using (MapiMessage message = MapiMessage.Load(inputPath, loadOptions))
            {
                // Save the message, preserving the original attachment formats
                message.Save(outputPath);
                Console.WriteLine($"Message saved to {outputPath} with embedded formats preserved.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
