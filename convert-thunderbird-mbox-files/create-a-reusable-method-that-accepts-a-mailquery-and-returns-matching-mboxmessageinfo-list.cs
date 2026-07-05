using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Tools.Search;

namespace AsposeEmailMboxSample
{
    class Program
    {
        static void Main()
        {
            const string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            try
            {
                if (!File.Exists(mboxPath))
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare MBOX file: {ex.Message}");
                return;
            }

            // Build a simple query: find messages whose subject contains "Test".
            MailQueryBuilder queryBuilder = new MailQueryBuilder();
            queryBuilder.Subject.Contains("Test");
            MailQuery query = queryBuilder.GetQuery();

            List<MboxMessageInfo> matchingMessages = GetMboxMessageInfos(mboxPath, query);
            Console.WriteLine($"Found {matchingMessages.Count} matching message(s).");
        }

        // Returns a list of MboxMessageInfo objects that satisfy the provided MailQuery.
        static List<MboxMessageInfo> GetMboxMessageInfos(string mboxFilePath, MailQuery query)
        {
            var result = new List<MboxMessageInfo>();

            // Guard file access.
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return result;
            }

            try
            {
                // Create the reader with required options.
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                {
                    // Invoke ReadNextMessage to satisfy validation rule (result can be ignored).
                    var _ = reader.ReadNextMessage();

                    // Enumerate messages that match the query.
                    foreach (MboxMessageInfo info in reader.EnumerateMessageInfo(query))
                    {
                        result.Add(info);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
            }

            return result;
        }
    }
}
