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
            // Output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // If the file already exists, delete it to avoid overwrite issues
            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            // Create a simple MapiMessage instance
            using (MapiMessage originalMessage = new MapiMessage(
                "sender@example.com",
                "receiver@example.com",
                "Sample Subject",
                "Sample body"))
            {
                // Save the message to a memory stream to obtain its byte representation
                using (MemoryStream tempStream = new MemoryStream())
                {
                    originalMessage.Save(tempStream);
                    byte[] messageBytes = tempStream.ToArray();

                    // Load a MapiMessage from the byte array
                    using (MemoryStream loadStream = new MemoryStream(messageBytes))
                    {
                        using (MapiMessage loadedMessage = MapiMessage.Load(loadStream))
                        {
                            // Add a custom Unicode property
                            string propertyName = "CustomProp";
                            string propertyValue = "Custom Value";
                            byte[] valueBytes = Encoding.Unicode.GetBytes(propertyValue);
                            loadedMessage.AddCustomProperty(MapiPropertyType.PT_UNICODE, valueBytes, propertyName);

                            // Persist the message as an MSG file
                            loadedMessage.Save(outputPath);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
