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
            string inputPath = "input.mht";
            string outputPath = "template.oft";

            // Ensure input MHTML file exists; create a minimal placeholder if missing.
            try
            {
                if (!File.Exists(inputPath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    using (StreamWriter writer = new StreamWriter(inputPath))
                    {
                        writer.WriteLine("<html><body><p>Placeholder MHTML content.</p></body></html>");
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // Load the MHTML file into a MailMessage.
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Convert MailMessage to MapiMessage.
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                {
                    // Save as Outlook File Template (OFT).
                    try
                    {
                        mapiMessage.SaveAsTemplate(outputPath);
                        Console.WriteLine($"OFT template saved to '{outputPath}'.");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Error saving OFT template: {saveEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
