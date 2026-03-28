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
            // Define output MSG file path
            string outputPath = Path.Combine(Environment.CurrentDirectory, "sample.msg");
            string outputDirectory = Path.GetDirectoryName(outputPath);

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Create a new MapiMessage with standard fields
            MapiMessage message = new MapiMessage(
                "alice@example.com",                     // From
                "bob@example.com;carol@example.com",    // To (multiple recipients separated by semicolon)
                "Sample Subject",                       // Subject
                "This is the body of the message.");    // Body

            // Save the message as an MSG file
            try
            {
                using (MapiMessage disposableMessage = message)
                {
                    disposableMessage.Save(outputPath);
                }
                Console.WriteLine($"MSG file saved successfully to '{outputPath}'.");
            }
            catch (Exception saveEx)
            {
                Console.Error.WriteLine($"Failed to save MSG file: {saveEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
