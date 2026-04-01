using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output MSG file paths
            string inputPath = "sample.msg";
            string outputPath = "sample_updated.msg";

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

                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Retrieve current priority
                int currentPriority = 0;
                if (msg.TryGetPropertyInt32(KnownPropertyList.Priority.Tag, ref currentPriority))
                {
                    Console.WriteLine($"Current priority: {currentPriority}");
                }
                else
                {
                    Console.WriteLine("Priority property not set.");
                }

                // Set a new priority (e.g., 1 = High)
                int newPriority = 1;
                msg.SetProperty(new MapiProperty(KnownPropertyList.Priority, newPriority));

                // Save the updated message
                msg.Save(outputPath);
                Console.WriteLine($"Message saved with new priority to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
