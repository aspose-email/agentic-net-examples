using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Guard file existence
            if (!File.Exists(inputPath))
            {
                // Create a minimal placeholder MSG if input is missing
                using (MapiMessage placeholder = new MapiMessage())
                {
                    placeholder.Save(outputPath);
                }
                Console.Error.WriteLine($"Input file not found: {inputPath}. Placeholder MSG created at {outputPath}.");
                return;
            }

            // Load the existing MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Set the importance to High using the known property descriptor
                message.SetProperty(KnownPropertyList.Importance, (int)MapiImportance.High);

                // Save the modified message back to MSG format
                message.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
