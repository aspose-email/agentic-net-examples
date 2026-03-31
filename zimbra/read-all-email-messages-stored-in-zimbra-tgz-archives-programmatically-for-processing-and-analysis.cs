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
            // Input TGZ archive path
            string tgzPath = "archive.tgz";
            // Output directory for extracted messages
            string outputDirectory = "ExtractedMessages";

            // Verify input file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Input file not found: {tgzPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Open the TGZ archive
            using (TgzReader tgzReader = new TgzReader(tgzPath))
            {
                // Optionally export all messages to the output directory
                try
                {
                    tgzReader.ExportTo(outputDirectory);
                }
                catch (Exception exportEx)
                {
                    Console.Error.WriteLine($"Export failed: {exportEx.Message}");
                    // Continue with manual iteration if export fails
                }

                // Iterate through each message in the archive
                while (true)
                {
                    try
                    {
                        // Read the next message; returns false when no more messages
                        bool hasMessage = tgzReader.ReadNextMessage();
                        if (!hasMessage)
                            break;
                    }
                    catch (Exception readEx)
                    {
                        Console.Error.WriteLine($"Error reading next message: {readEx.Message}");
                        break;
                    }

                    // Retrieve the current message
                    MailMessage currentMessage = tgzReader.CurrentMessage;
                    if (currentMessage == null)
                        continue;

                    // Process the message (e.g., display basic info)
                    Console.WriteLine($"Subject: {currentMessage.Subject}");
                    Console.WriteLine($"From: {currentMessage.From}");
                    Console.WriteLine($"To: {currentMessage.To}");

                    // Save the message as an .eml file
                    string safeSubject = string.IsNullOrWhiteSpace(currentMessage.Subject) ? "Untitled" : currentMessage.Subject;
                    // Replace invalid filename characters
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        safeSubject = safeSubject.Replace(invalidChar, '_');
                    }
                    string emlPath = Path.Combine(outputDirectory, $"{safeSubject}.eml");

                    try
                    {
                        using (MailMessage messageToSave = currentMessage)
                        {
                            messageToSave.Save(emlPath, SaveOptions.DefaultEml);
                        }
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
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
