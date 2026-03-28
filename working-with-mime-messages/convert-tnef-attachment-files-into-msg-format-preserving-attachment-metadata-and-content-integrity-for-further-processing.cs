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
            // Define input TNEF file and output MSG file paths
            string inputPath = "input.tnef";
            string outputPath = "output.msg";

            // Verify that the input TNEF file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load the TNEF file into a MapiMessage and save it as MSG
            try
            {
                using (MapiMessage message = MapiMessage.LoadFromTnef(inputPath))
                {
                    // Save as MSG using the default MSG save options
                    message.Save(outputPath, SaveOptions.DefaultMsg);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing TNEF file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
