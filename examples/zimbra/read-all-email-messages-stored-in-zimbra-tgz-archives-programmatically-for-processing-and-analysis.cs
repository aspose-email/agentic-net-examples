using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        List<MailMessage> messages = new List<MailMessage>();

        try
        {
            string tgzPath = "archive.tgz";
            string outputDirectory = "ExtractedMessages";

            // Verify the TGZ archive exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error creating output directory: {ex.Message}");
                return;
            }

            // Open the TGZ reader and process each message
            try
            {
                using (TgzReader tgzReader = new TgzReader(tgzPath))
                {
                    while (true)
                    {
                        bool hasMessage;
                        try
                        {
                            hasMessage = tgzReader.ReadNextMessage();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error reading next message: {ex.Message}");
                            break;
                        }

                        if (!hasMessage)
                        {
                            break; // No more messages
                        }

                        // CurrentMessage returns a MailMessage instance
                        using (MailMessage mailMessage = tgzReader.CurrentMessage)
                        {
                            try
                            {
                                Console.WriteLine($"Subject: {mailMessage.Subject}");

                                // Create a safe filename based on the subject
                                string safeSubject = string.IsNullOrWhiteSpace(mailMessage.Subject)
                                    ? "NoSubject"
                                    : string.Concat(mailMessage.Subject.Split(Path.GetInvalidFileNameChars()));

                                string outputPath = Path.Combine(outputDirectory,
                                    $"{safeSubject}_{Guid.NewGuid()}.eml");

                                // Save the message to an .eml file
                                mailMessage.Save(outputPath, SaveOptions.DefaultEml);
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Error processing message: {ex.Message}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error initializing TgzReader: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}