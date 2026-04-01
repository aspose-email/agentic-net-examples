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
            string outputPath = "output.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a simple MAPI message
            using (MapiMessage msg = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Sample Subject",
                "This is the body of the message."))
            {
                // Add a custom metadata property (Unicode string)
                byte[] customValue = Encoding.UTF8.GetBytes("CustomValue");
                msg.AddCustomProperty(MapiPropertyType.PT_UNICODE, customValue, "X-Custom-Property");

                // Save the message to file
                try
                {
                    msg.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
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
