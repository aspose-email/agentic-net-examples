using Aspose.Email.Tools.Search;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Build a simple query (example: messages from a specific address)
            MailQueryBuilder builder = new MailQueryBuilder();
            builder.From.Contains("example@example.com", true);
            MailQuery query = builder.GetQuery();

            List<MboxMessageInfo> messages = GetMboxMessages(mboxPath, query);

            foreach (MboxMessageInfo info in messages)
            {
                Console.WriteLine($"Subject: {info.Subject}");
                Console.WriteLine($"From: {info.From}");
                Console.WriteLine($"To: {info.To}");
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Returns a list of MboxMessageInfo objects that match the provided MailQuery.
    static List<MboxMessageInfo> GetMboxMessages(string mboxFilePath, MailQuery query)
    {
        var result = new List<MboxMessageInfo>();

        try
        {
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
            {
                // Invoke ReadNextMessage to satisfy the required usage pattern.
                MailMessage _ = reader.ReadNextMessage();

                // Enumerate message infos that satisfy the query.
                foreach (MboxMessageInfo info in reader.EnumerateMessageInfo(query))
                {
                    result.Add(info);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to read MBOX file: {ex.Message}");
        }

        return result;
    }
}
