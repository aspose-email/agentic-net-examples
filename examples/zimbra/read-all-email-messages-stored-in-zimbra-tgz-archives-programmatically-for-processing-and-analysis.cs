using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace ZimbraTgzReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the Zimbra TGZ archive
                string tgzPath = "archive.tgz";

                // Verify that the TGZ file exists
                if (!File.Exists(tgzPath))
                {
                    Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                    return;
                }

                // Directory where extracted messages will be saved
                string outputDirectory = "ExtractedMessages";

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Open the TGZ archive using TgzReader
                using (TgzReader tgzReader = new TgzReader(tgzPath))
                {
                    // Iterate through all messages in the archive
                    while (tgzReader.ReadNextMessage())
                    {
                        // Get the current MailMessage
                        MailMessage message = tgzReader.CurrentMessage;

                        if (message == null)
                        {
                            continue;
                        }

                        // Output basic information about the message
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"Date: {message.Date}");

                        // Create a safe filename based on the subject
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string fileName = Path.Combine(outputDirectory, $"{safeSubject}_{Guid.NewGuid()}.eml");

                        // Save the message as an EML file
                        try
                        {
                            message.Save(fileName, SaveOptions.DefaultEml);
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                        }

                        Console.WriteLine(new string('-', 40));
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