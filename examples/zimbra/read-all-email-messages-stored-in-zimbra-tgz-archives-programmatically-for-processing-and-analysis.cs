using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

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
                    // Get the current message
                    MailMessage message = tgzReader.CurrentMessage;

                    // Create a safe filename based on the subject (fallback if subject is empty)
                    string subject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        subject = subject.Replace(invalidChar, '_');
                    }

                    // Append a GUID to avoid filename collisions
                    string fileName = Path.Combine(outputDirectory, $"{subject}_{Guid.NewGuid()}.eml");

                    // Save the message as an EML file
                    message.Save(fileName, SaveOptions.DefaultEml);
                }
            }
        }
        catch (Exception ex)
        {
            // Output any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}