using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define source and target file paths
            string sourcePath = "source.eml";
            string targetPath = "target.oft";

            // Ensure the source EML file exists; create a minimal placeholder if missing
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                // Save the message as an Outlook Template (OFT) using default options
                try
                {
                    mailMessage.Save(targetPath, SaveOptions.DefaultOft);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Error saving OFT file: {saveEx.Message}");
                    return;
                }
            }

            Console.WriteLine("EML successfully converted to OFT.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
