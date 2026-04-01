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
            string inputPath = "input.eml";
            string outputPath = "output.oft";

            // Ensure input file exists; create a minimal placeholder if missing
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

                try
                {
                    string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            try
            {
                using (MailMessage mailMessage = MailMessage.Load(inputPath))
                {
                    // Convert to MAPI message
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Save as Outlook File Template (OFT)
                        try
                        {
                            mapiMessage.SaveAsTemplate(outputPath);
                            Console.WriteLine($"Successfully saved OFT file to '{outputPath}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save OFT file: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or process EML file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
