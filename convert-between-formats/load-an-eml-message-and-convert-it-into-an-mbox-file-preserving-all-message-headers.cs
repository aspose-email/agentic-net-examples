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
            string emlPath = "input.eml";
            string mboxPath = "output.mbox";

            // Ensure the input EML file exists; create a minimal placeholder if missing.
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                string placeholderContent = "From: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                try
                {
                    File.WriteAllText(emlPath, placeholderContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
            string outputDirectory = Path.GetDirectoryName(mboxPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the EML message.
            MailMessage emlMessage;
            try
            {
                emlMessage = MailMessage.Load(emlPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load EML file: {ex.Message}");
                return;
            }

            // Convert and save as MBOX, preserving all headers.
            try
            {
                using (FileStream mboxStream = new FileStream(mboxPath, FileMode.Create, FileAccess.Write))
                {
                    MboxSaveOptions saveOptions = new MboxSaveOptions();
                    using (MboxrdStorageWriter writer = new MboxrdStorageWriter(mboxStream, saveOptions))
                    {
                        writer.WriteMessage(emlMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write MBOX file: {ex.Message}");
                return;
            }

            Console.WriteLine("EML successfully converted to MBOX.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
