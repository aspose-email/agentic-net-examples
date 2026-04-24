using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace AsposeEmailMboxIndex
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the MBOX file
                string mboxPath = "archive.mbox";

                // Verify that the MBOX file exists
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                // Create the MBOX reader using the factory method (concrete implementation returned)
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    // Dictionary to hold date -> list of message entry IDs
                    Dictionary<DateTime, List<string>> dateIndex = new Dictionary<DateTime, List<string>>();

                    // Enumerate all message info objects in the storage
                    foreach (MboxMessageInfo messageInfo in mboxReader.EnumerateMessageInfo())
                    {
                        DateTime messageDate = messageInfo.Date;
                        string entryId = messageInfo.EntryId;

                        if (!dateIndex.ContainsKey(messageDate))
                        {
                            dateIndex[messageDate] = new List<string>();
                        }

                        dateIndex[messageDate].Add(entryId);
                    }

                    // Example output: display the number of messages per date
                    foreach (KeyValuePair<DateTime, List<string>> kvp in dateIndex)
                    {
                        Console.WriteLine($"{kvp.Key:yyyy-MM-dd}: {kvp.Value.Count} message(s)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
