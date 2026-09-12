using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailNoteExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Input MSG file containing the note
                string inputPath = "input.msg";
                // Output MSG file for the new note document
                string outputPath = "output_note.msg";

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

                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the existing MSG file as a MapiMessage
                MapiMessage originalMessage = MapiMessage.Load(inputPath);

                // Preserve the message content and metadata by saving it as a new MSG file
                originalMessage.Save(outputPath);

                Console.WriteLine($"Note document saved successfully to: {outputPath}");
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
