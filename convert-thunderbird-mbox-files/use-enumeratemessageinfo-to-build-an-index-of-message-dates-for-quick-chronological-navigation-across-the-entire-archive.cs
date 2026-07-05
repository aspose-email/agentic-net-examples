using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Path to the source MBOX file
        string mboxPath = "input.mbox";

        // Path where the date index will be saved
        string indexOutputPath = "output/message_dates.txt";

        // Verify that the MBOX file exists
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
            return;
        }

        // Ensure the output directory exists
        string outputDir = Path.GetDirectoryName(indexOutputPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        try
        {
            // Open the MBOX storage for reading
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Build an index of message dates (chronological navigation)
                List<(int Index, DateTime Date)> dateIndex = new List<(int, DateTime)>();
                int idx = 0;
                foreach (MboxMessageInfo msgInfo in mboxReader.EnumerateMessageInfo())
                {
                    DateTime msgDate = msgInfo.Date;
                    dateIndex.Add((idx, msgDate));
                    idx++;
                }

                // Write the index to a text file (tab‑separated: index, ISO‑8601 date)
                using (StreamWriter writer = new StreamWriter(indexOutputPath))
                {
                    foreach ((int Index, DateTime Date) entry in dateIndex)
                    {
                        writer.WriteLine($"{entry.Index}\t{entry.Date:O}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MBOX: {ex.Message}");
        }
    }
}
