using System;
using System.IO;
using Aspose.Email;

namespace SaveEmailToEml
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string sourcePath = "source.eml";
                string destinationPath = "saved.eml";

                // Ensure source file exists, create placeholder if missing
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
                        string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.";
                        File.WriteAllText(sourcePath, placeholder);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder source file: {ex.Message}");
                        return;
                    }
                }

                // Load the email message
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(sourcePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load email message: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    // Prepare save options for EML format
                    EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);

                    // Ensure destination directory exists
                    string destDirectory = Path.GetDirectoryName(destinationPath);
                    if (!string.IsNullOrEmpty(destDirectory) && !Directory.Exists(destDirectory))
                    {
                        try
                        {
                            Directory.CreateDirectory(destDirectory);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create destination directory: {ex.Message}");
                            return;
                        }
                    }

                    // Save the message preserving all headers and body
                    try
                    {
                        mailMessage.Save(destinationPath, emlSaveOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save email message: {ex.Message}");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
