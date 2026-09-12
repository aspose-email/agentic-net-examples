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
            const string inputPath = "input.msg";
            const string outputPath = "output.msg";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a simple MailMessage and save it as MSG to serve as placeholder
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To = "receiver@example.com";
                        placeholder.Subject = "Placeholder Message";
                        placeholder.Body = "This is a placeholder MSG file.";
                        placeholder.Save(inputPath, SaveOptions.DefaultMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file '{inputPath}': {ex.Message}");
                return;
            }

            // Save the MSG file; attachments (if any) are automatically persisted
            try
            {
                msg.Save(outputPath);
                Console.WriteLine($"Message saved successfully to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save MSG file '{outputPath}': {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
