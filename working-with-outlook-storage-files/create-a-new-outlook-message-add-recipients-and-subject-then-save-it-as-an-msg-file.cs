using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define output MSG file path
                string outputPath = "output.msg";
                string outputDirectory = Path.GetDirectoryName(outputPath);

                // Ensure the output directory exists
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Create a new MAPI message
                using (MapiMessage message = new MapiMessage())
                {
                    // Set basic properties
                    message.Subject = "Sample Subject";
                    message.Body = "This is the body of the Outlook message.";

                    // Add first recipient
                    message.Recipients.Add("Alice", "alice@example.com", MapiRecipientType.MAPI_TO);

                    // Add second recipient
                    message.Recipients.Add("Bob", "bob@example.com", MapiRecipientType.MAPI_TO);

                    // Save the message as MSG file
                    message.Save(outputPath);
                    Console.WriteLine($"Message successfully saved to '{outputPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
