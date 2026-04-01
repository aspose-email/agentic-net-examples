using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define output file path
            string outputPath = "customMessage.msg";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a simple MAPI message
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "receiver@example.com",
                "Test Subject",
                "Test Body"))
            {
                // Define custom property name and value
                string propertyName = "MyCustomProp";
                string propertyValue = "CustomValue";

                // Convert the string value to a byte array (Unicode encoding) for PT_UNICODE type
                byte[] propertyData = Encoding.Unicode.GetBytes(propertyValue);

                // Add the custom property to the message using the recommended API
                message.AddCustomProperty(MapiPropertyType.PT_UNICODE, propertyData, propertyName);

                // Save the message to disk with error handling
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
