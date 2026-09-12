using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Sample code for filtering MBOX messages by sender.
            const string mboxPath = "storage.mbox";
            const string senderEmail = "sender@example.com";
            const string outputDir = "output";

            // Ensure the output directory exists.
            Directory.CreateDirectory(outputDir);

            // Verify that the MBOX file exists before attempting to read it.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Create the MBOX reader.
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Iterate through each message info object.
                foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
                {
                    // Filter: process only messages from the designated sender.
                    if (mboxMessageInfo.From != null &&
                        !string.IsNullOrEmpty(mboxMessageInfo.From.Address) &&
                        mboxMessageInfo.From.Address.IndexOf(senderEmail, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine($"Subject: {mboxMessageInfo.Subject}");
                        Console.WriteLine($"From: {mboxMessageInfo.From}");
                        Console.WriteLine($"To: {mboxMessageInfo.To}");

                        // Extract the full MIME message.
                        MailMessage eml = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions());

                        // Create a safe file name for the saved .eml file.
                        string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? "NoSubject" : eml.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string emlPath = Path.Combine(outputDir, $"{safeSubject}.eml");

                        // Save the extracted message.
                        eml.Save(emlPath);
                        Console.WriteLine($"Saved to: {emlPath}");
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
