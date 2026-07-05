using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        // Paths for source and target files
        string sourcePath = "input.eml";
        string targetPath = "output.emlx";

        // If the source file does not exist, create a simple placeholder message
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
                using (MailMessage placeholderMessage = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Placeholder Subject",
                    "Placeholder body."))
                {
                    placeholderMessage.Save(sourcePath, SaveOptions.DefaultEml);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                return;
            }

            Console.Error.WriteLine($"Source file not found. Placeholder created at: {sourcePath}");
            return;
        }

        // Ensure the output directory exists
        string targetDir = Path.GetDirectoryName(targetPath);
        if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
        {
            Directory.CreateDirectory(targetDir);
        }

        try
        {
            // Load the existing email message
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                // Add or modify a custom header without using ContainsKey
                if (mailMessage.Headers["X-Custom-Header"] != null)
                {
                    mailMessage.Headers["X-Custom-Header"] = "ModifiedValue";
                }
                else
                {
                    mailMessage.Headers.Add("X-Custom-Header", "CustomValue");
                }

                // Save the modified message as an EMLX file
                EmlSaveOptions emlOptions = SaveOptions.DefaultEmlx;
                mailMessage.Save(targetPath, emlOptions);
            }

            Console.WriteLine($"Message saved successfully to {targetPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
