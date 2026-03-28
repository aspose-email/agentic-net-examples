using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Input MSG file path
            string inputPath = "sample.msg";

            // Desired output path (EML format in this example)
            string outputPath = "output.eml";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            // Load the MSG file into a MailMessage instance
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Save the message as EML using the default EML save options
                message.Save(outputPath, SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            // Friendly error output
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
