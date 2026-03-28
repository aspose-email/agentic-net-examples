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
            string outputPath = "sample.msg";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the MSG message with required fields
            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test Subject",
                "This is a plain text body.",
                OutlookMessageFormat.Unicode))
            {
                // Add a custom header
                message.Headers["X-Custom-Header"] = "CustomValue";

                // Save the message to file
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine("Message saved to " + outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to save message: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
