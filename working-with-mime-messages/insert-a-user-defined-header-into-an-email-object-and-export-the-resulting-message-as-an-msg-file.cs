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
            // Define output MSG file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MAPI message and add a custom header
            using (MapiMessage message = new MapiMessage())
            {
                message.Subject = "Sample Message";
                message.Body = "This is a sample email body.";
                // Insert user-defined header
                message.Headers["X-Custom-Header"] = "MyValue";

                // Save the message as MSG file
                message.Save(outputPath);
            }

            Console.WriteLine("Message saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
