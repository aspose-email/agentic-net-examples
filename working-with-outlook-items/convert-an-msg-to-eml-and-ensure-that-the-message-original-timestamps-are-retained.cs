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
            string inputPath = "input.msg";
            string outputPath = "output.eml";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "placeholder@example.com",
                    "recipient@example.com",
                    "Placeholder",
                    "This is a placeholder message."))
                {
                    placeholder.Save(inputPath);
                }
                Console.WriteLine($"Placeholder MSG created at {inputPath}");
            }

            // Load the MSG file, convert to MailMessage, and save as EML while preserving original timestamps.
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                {
                    mail.Save(outputPath);
                }
            }

            Console.WriteLine($"MSG successfully converted to EML: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
