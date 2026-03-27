using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Error: File not found – {mboxPath}");
                return;
            }

            // Create a reader for the MBOX file with default load options
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Iterate through each message info entry in the MBOX storage
                foreach (MboxMessageInfo mboxMessageInfo in mboxReader.EnumerateMessageInfo())
                {
                    Console.WriteLine($"Subject: {mboxMessageInfo.Subject}");
                    Console.WriteLine($"From: {mboxMessageInfo.From}");
                    Console.WriteLine($"To: {mboxMessageInfo.To}");

                    // Extract the full MIME message using the entry ID
                    using (MailMessage eml = mboxReader.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions()))
                    {
                        // Use a safe filename based on the subject
                        string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? "Untitled" : eml.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string fileName = $"{safeSubject}.eml";

                        // Save the extracted message as an .eml file
                        eml.Save(fileName);
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
