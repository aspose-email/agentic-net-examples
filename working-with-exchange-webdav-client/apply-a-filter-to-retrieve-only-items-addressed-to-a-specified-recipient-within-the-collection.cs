using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";
            const string recipient = "recipient@example.com";
            const string outputDir = "output";

            // Verify the MBOX file exists before attempting to read it.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputDir))
                Directory.CreateDirectory(outputDir);

            // Create the MBOX reader.
            using var mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

            // Build a query that selects messages where the 'To' field contains the specified recipient.
            var mailQuery = new MailQuery($"('To' Contains '{recipient}')");

            // Enumerate matching messages using default EML load options.
            foreach (MailMessage message in mbox.EnumerateMessages(new EmlLoadOptions(), mailQuery))
            {
                try
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {message.To}");

                    // Create a safe file name from the subject.
                    string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "Untitled" : message.Subject;
                    foreach (char c in Path.GetInvalidFileNameChars())
                        safeSubject = safeSubject.Replace(c, '_');

                    string emlPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                    // Save the message as an .eml file.
                    message.Save(emlPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to process a message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
