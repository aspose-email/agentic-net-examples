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
                using (MapiMessage placeholder = new MapiMessage())
                {
                    placeholder.Subject = "Placeholder";
                    placeholder.Body = "This is a placeholder message.";
                    placeholder.Save(inputPath);
                }
                Console.WriteLine($"Placeholder MSG created at '{inputPath}'.");
            }

            // Load the MSG file.
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                Console.WriteLine($"Subject: {msg.Subject}");
                Console.WriteLine($"From: {msg.SenderEmailAddress}");

                // Business logic: convert to a MailMessage and save as EML.
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                {
                    mail.Save(outputPath);
                }

                Console.WriteLine($"Converted message saved as EML at '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
