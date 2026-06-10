using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the Zimbra TGZ archive
            string tgzPath = "archive.tgz";

            // Verify that the TGZ file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Input file not found: {tgzPath}");
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
                // Get total number of messages in the archive
                int totalMessages = tgzReader.GetTotalItemsCount();
                Console.WriteLine($"Total messages in archive: {totalMessages}");

                for (int index = 0; index < totalMessages; index++)
                {
                    // Read the next message
                    tgzReader.ReadNextMessage();

                    // Retrieve the current MailMessage
                    MailMessage currentMessage = tgzReader.CurrentMessage;
                    if (currentMessage == null)
                    {
                        continue;
                    }

                    // Display basic information
                    Console.WriteLine($"Message {index + 1}:");
                    Console.WriteLine($"  Subject: {currentMessage.Subject}");
                    Console.WriteLine($"  From: {currentMessage.From}");
                    Console.WriteLine($"  To: {currentMessage.To}");

                    // Save the message as an .eml file
                    string safeSubject = string.IsNullOrWhiteSpace(currentMessage.Subject) ? $"Message_{index + 1}" : currentMessage.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(invalidChar, '_');
                    }

                    string emlPath = Path.Combine(outputDirectory, $"{safeSubject}.eml");
                    try
                    {
                        currentMessage.Save(emlPath);
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save message '{safeSubject}': {saveEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
