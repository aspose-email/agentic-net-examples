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
            string inputMboxPath = "input.mbox";
            string outputFolderPath = "output";

            // Verify input file exists
            if (!File.Exists(inputMboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {inputMboxPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Example: read a few messages using ReadNextMessage()
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions()))
            {
                int readCount = 0;
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null && readCount < 5)
                {
                    Console.WriteLine($"Read message subject: {message.Subject}");
                    readCount++;
                }
            }

            // Split the MBOX into smaller parts (e.g., 10 MB each)
            using (MboxStorageReader splitter = MboxStorageReader.CreateReader(inputMboxPath, new MboxLoadOptions()))
            {
                long chunkSizeBytes = 10L * 1024 * 1024; // 10 MB
                splitter.SplitInto(chunkSizeBytes, outputFolderPath);
                Console.WriteLine($"MBOX split completed. Parts are stored in: {outputFolderPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
