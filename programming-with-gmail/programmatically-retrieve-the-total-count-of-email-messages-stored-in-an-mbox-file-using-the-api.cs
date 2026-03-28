using System;
using System.IO;
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
                // Create an empty placeholder MBOX file if it does not exist
                using (FileStream placeholder = File.Create(mboxPath))
                {
                    // No content needed for placeholder
                }
                Console.Error.WriteLine($"Input file not found. Created empty placeholder at {mboxPath}.");
                return;
            }

            // Open the MBOX file using the Aspose.Email MboxStorageReader
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
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
