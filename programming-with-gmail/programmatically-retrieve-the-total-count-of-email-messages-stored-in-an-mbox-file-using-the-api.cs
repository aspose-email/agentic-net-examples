using System;
using System.IO;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxFilePath = "sample.mbox";

            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return;
            }

            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
            {
                int totalCount = reader.GetTotalItemsCount();
                Console.WriteLine($"Total messages in MBOX: {totalCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
