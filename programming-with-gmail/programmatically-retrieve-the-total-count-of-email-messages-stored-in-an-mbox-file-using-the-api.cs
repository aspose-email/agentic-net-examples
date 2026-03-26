using System.Collections.Generic;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

namespace AsposeEmailMboxCount
{
    class Program
    {
        static void Main(string[] args)
        {
        List<MailMessage> messages = new List<MailMessage>();

            try
            {
                // Path to the MBOX file
                string mboxFilePath = "storage.mbox";

                // Verify that the file exists before attempting to read it
                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                    return;
                }

                // Create a reader for the MBOX file and retrieve the total message count
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                {
                    int totalMessages = mboxReader.GetTotalItemsCount();
                    Console.WriteLine($"Total messages in the MBOX file: {totalMessages}");
                }
            }
            catch (Exception ex)
            {
                // Output any unexpected errors to the error console
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}