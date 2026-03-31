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

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Modify the first recipient if any
                if (message.Recipients != null && message.Recipients.Count > 0)
                {
                    message.Recipients[0].EmailAddress = "modified@example.com";
                }

                // Add a new TO recipient
                message.Recipients.Add(
                    address: "newrecipient@example.com",
                    displayName: "New Recipient",
                    recipientType: MapiRecipientType.MAPI_TO);

                // Save the updated MSG file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
