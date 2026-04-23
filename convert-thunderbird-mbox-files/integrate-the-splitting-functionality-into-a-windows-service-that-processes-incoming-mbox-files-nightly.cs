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
            string inputMboxPath = "incoming.mbox";
            string outputFolderPath = "SplitParts";

            // Guard file existence
            if (!File.Exists(inputMboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {inputMboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolderPath))
            {
                try
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Process the MBOX file
            try
            {
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions()))
                {
                    // Example: read the first message using ReadNextMessage()
                    MailMessage firstMessage = reader.ReadNextMessage();
                    if (firstMessage != null)
                    {
                        Console.WriteLine($"First message subject: {firstMessage.Subject}");
                    }
                    else
                    {
                        Console.WriteLine("No messages found in the MBOX file.");
                    }

                    // Split the MBOX into parts of 10 MB each
                    long maxPartSize = 10L * 1024 * 1024; // 10 MB
                    reader.SplitInto(maxPartSize, outputFolderPath);
                    Console.WriteLine($"MBOX split completed. Parts are stored in: {outputFolderPath}");
                }
            }
            catch (Exception mboxEx)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {mboxEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
