using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";

            // Verify the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the HTML file as a MailMessage
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load HTML file: {loadEx.Message}");
                return;
            }

            // Ensure the MailMessage is disposed properly
            using (message)
            {
                string outputPath = "output.mhtml";

                // Save the message in MHTML format (as an example of format conversion)
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMhtml);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
