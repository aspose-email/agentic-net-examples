using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace UniqueSendersCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                const string mboxPath = "storage.mbox";

                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                var uniqueSenders = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                using (MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    foreach (MboxMessageInfo messageInfo in mbox.EnumerateMessageInfo())
                    {
                        // 'From' is a MailAddress; extract the address string.
                        string? sender = messageInfo.From?.Address ?? messageInfo.From?.ToString();

                        if (!string.IsNullOrEmpty(sender))
                        {
                            uniqueSenders.Add(sender);
                        }
                    }
                }

                Console.WriteLine($"Total unique senders: {uniqueSenders.Count}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
