using System;
using System.IO;
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
                // Define output MSG file path
                string outputPath = "output.msg";

                // Ensure the output directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create a new MSG message with standard headers and plain text body
                using (MapiMessage message = new MapiMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Subject",
                    "This is the plain text body of the message."))
                {
                    // Save the message to the specified file
                    message.Save(outputPath);
                }

                Console.WriteLine($"Message saved to {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
