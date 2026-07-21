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
            // Author note: Example demonstrates filtering messages in an MBOX file by subject.
            string mboxPath = "storage.mbox";

            // Verify that the MBOX file exists before attempting to read it.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Create the MBOX reader.
            using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Define a query that selects messages whose subject contains the word "Invoice".
                MailQuery subjectQuery = new MailQuery("Subject Contains 'Invoice'");

                // Enumerate only the messages that match the query.
                foreach (MailMessage message in mbox.EnumerateMessages(subjectQuery))
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {string.Join(", ", message.To)}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
