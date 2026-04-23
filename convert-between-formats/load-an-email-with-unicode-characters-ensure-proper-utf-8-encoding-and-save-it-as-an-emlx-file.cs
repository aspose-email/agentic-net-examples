using System;
using System.IO;
using System.Text;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "unicode_email.eml";
            string outputPath = "unicode_email.emlx";

            // Verify input file exists; create a minimal placeholder if missing
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
                    using (FileStream placeholderStream = File.Create(inputPath))
                    {
                        string placeholderContent = "Subject: =?utf-8?B?5L2g5aW9?=\r\n\r\nこんにちは世界";
                        byte[] bytes = Encoding.UTF8.GetBytes(placeholderContent);
                        placeholderStream.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the email with UTF-8 encoding preference
            EmlxLoadOptions loadOptions = new EmlxLoadOptions();
            loadOptions.PreferredTextEncoding = Encoding.UTF8;

            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(inputPath, loadOptions);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email: {ex.Message}");
                return;
            }

            using (mailMessage)
            {
                // Save as EMLX format
                EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);
                try
                {
                    mailMessage.Save(outputPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email as EMLX: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
