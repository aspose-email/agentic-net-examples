using Aspose.Email;
using Aspose.Email.Mapi;
using System;
using System.IO;

namespace AppendCcToMsg
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define input and output file paths.
                string inputPath = "input.msg";
                string outputPath = "output.msg";

                // Guard input file existence.
                if (!File.Exists(inputPath))
                {
                    // Create a placeholder MSG file if it does not exist.
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

                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure the output directory exists.
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Load the existing MSG file.
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Add new CC recipients. Use numeric cast for CC type to avoid enum name differences.
                    const MapiRecipientType ccType = (MapiRecipientType)2; // 2 corresponds to CC in MAPI
                    message.Recipients.Add("alice@example.com", "Alice", ccType);
                    message.Recipients.Add("bob@example.com", "Bob", ccType);

                    // Save the modified message.
                    message.Save(outputPath);
                }

                Console.WriteLine($"Message saved with new CC recipients to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
