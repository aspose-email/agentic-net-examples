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
            // Path to the source MBOX file.
            string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the output directory exists.
            string outputDir = "FilteredMessages";
            Directory.CreateDirectory(outputDir);

            // Create the MBOX reader.
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Define filter criteria: messages from example.com with "Report" in the subject.
                MailQuery mailQuery = new MailQuery("(('From' Contains 'example.com') & 'Subject' Contains 'Report')");

                // Enumerate and process only the messages that match the query.
                foreach (MailMessage message in mbox.EnumerateMessages(new EmlLoadOptions(), mailQuery))
                {
                    using (message)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"From: {message.From}");
                        Console.WriteLine($"To: {string.Join(", ", message.To)}");

                        // Prepare a safe file name.
                        string safeSubject = string.IsNullOrWhiteSpace(message.Subject) ? "NoSubject" : message.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string filePath = Path.Combine(outputDir, $"{safeSubject}.eml");

                        try
                        {
                            message.Save(filePath);
                            Console.WriteLine($"Saved to: {filePath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message '{safeSubject}': {saveEx.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
