using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string sourcePath = "source.eml";
            const string targetPath = "target.eml";

            // Ensure the source file exists; create a minimal placeholder if missing.
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
                    string placeholder = "From: placeholder@example.com\r\nTo: placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(sourcePath, placeholder);
                    Console.WriteLine($"Created placeholder source file at '{sourcePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder source file: {ex.Message}");
                    return;
                }
            }

            // Load the email message.
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(sourcePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email from '{sourcePath}': {ex.Message}");
                return;
            }

            // Ensure the target directory exists.
            try
            {
                string targetDir = Path.GetDirectoryName(Path.GetFullPath(targetPath));
                if (!string.IsNullOrEmpty(targetDir) && !Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare target directory: {ex.Message}");
                return;
            }

            // Save the message as EML preserving embedded message format.
            try
            {
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat)
                {
                    PreserveEmbeddedMessageFormat = true
                };
                mailMessage.Save(targetPath, emlSaveOptions);
                Console.WriteLine($"Email saved successfully to '{targetPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save email to '{targetPath}': {ex.Message}");
            }
            finally
            {
                // Dispose the loaded message.
                if (mailMessage != null)
                {
                    mailMessage.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
