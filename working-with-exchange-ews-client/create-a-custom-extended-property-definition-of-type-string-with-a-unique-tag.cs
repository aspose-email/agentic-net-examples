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
            // Define output file path
            string outputPath = "CustomPropertyMessage.msg";

            // Ensure the directory for the output file exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI message
            using (MapiMessage message = new MapiMessage())
            {
                // Set basic properties
                message.Subject = "Message with Custom Extended Property";
                message.Body = "This message contains a custom string property.";

                // Define a unique property tag (example: 0x8000)
                const int customTag = 0x8000;
                string propertyName = "MyCustomString";
                string propertyValue = "Custom value";

                // Encode the string value as Unicode bytes
                byte[] valueBytes = Encoding.Unicode.GetBytes(propertyValue);

                // Add the custom property (type string = PT_UNICODE)
                message.AddCustomProperty(
                    MapiPropertyType.PT_UNICODE,
                    valueBytes,
                    propertyName);

                // Save the message to a file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
