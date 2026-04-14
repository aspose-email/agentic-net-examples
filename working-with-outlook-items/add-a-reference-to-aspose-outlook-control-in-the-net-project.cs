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
            // Define the output MSG file path
            string outputPath = "sample.msg";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a simple MAPI message
            using (MapiMessage message = new MapiMessage(
                "from@example.com",
                "to@example.com",
                "Test Subject",
                "This is the body of the message."))
            {
                // Save the message as an Outlook MSG file
                message.Save(outputPath);
                Console.WriteLine($"Message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            // Log any errors to the error console
            Console.Error.WriteLine(ex.Message);
        }
    }
}
