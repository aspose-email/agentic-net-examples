using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input and output file paths
                string inputPath = "sample.msg";
                string outputPath = "sample_updated.msg";

                // Ensure the output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Verify the input file exists; create a minimal placeholder if missing
                if (!File.Exists(inputPath))
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Save(inputPath);
                    }
                }

                // Load the message, add a custom metadata property, and save the updated message
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Encode the custom value as UTF-8 bytes
                    byte[] customData = Encoding.UTF8.GetBytes("CustomValue");

                    // Add a custom property with a user-defined name
                    message.AddCustomProperty(MapiPropertyType.PT_UNICODE, customData, "X-Custom-Property");

                    // Save the modified message to the output file
                    message.Save(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
