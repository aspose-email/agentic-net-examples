using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output MSG file path
            string outputPath = "ProcessedMessage.msg";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDir}': {dirEx.Message}");
                    return;
                }
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage("sender@example.com", "recipient@example.com", "Sample Subject", "This is the body of the message."))
            {
                // Prepare the processing timestamp value
                string timestamp = DateTime.UtcNow.ToString("o"); // ISO 8601 format

                // Add a custom Unicode property named "ProcessingTimestamp"
                try
                {
                    message.AddCustomProperty(
                        MapiPropertyType.PT_UNICODE,
                        Encoding.Unicode.GetBytes(timestamp),
                        "ProcessingTimestamp");
                }
                catch (Exception propEx)
                {
                    Console.Error.WriteLine($"Failed to add custom property: {propEx.Message}");
                    return;
                }

                // Save the message to the specified file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with custom property to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
