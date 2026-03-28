using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output paths
            string emlPath = "input.eml";
            string mboxPath = "output.mbox";

            // Ensure the input EML file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (var writer = new StreamWriter(emlPath))
                    {
                        writer.WriteLine("From: placeholder@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine("Subject: Placeholder");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder EML message.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML message
            MailMessage message;
            try
            {
                message = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Write the message to an MBOX file using a concrete writer
            try
            {
                var saveOptions = new MboxSaveOptions();
                using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxPath, saveOptions))
                {
                    writer.WriteMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write MBOX file: {ex.Message}");
                return;
            }
            finally
            {
                // Dispose the loaded message
                if (message != null)
                    message.Dispose();
            }

            Console.WriteLine("Conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
