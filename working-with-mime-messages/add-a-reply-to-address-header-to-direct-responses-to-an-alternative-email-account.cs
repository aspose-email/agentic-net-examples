using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "MessageWithReplyTo.msg");

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MapiMessage message = new MapiMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test Subject",
                "This is the body of the message."))
            {
                // Set the Reply-To address (semicolon-separated if multiple)
                message.ReplyTo = "alt-reply@example.com";

                // Save the message to a file
                message.Save(outputPath);
                Console.WriteLine($"Message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
