using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for the source MBOX file and the folder where split parts will be saved
            string mboxFilePath = "large_thunderbird.mbox";
            string outputDirectory = "SplitParts";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure the output directory exists
            try
            {
                Directory.CreateDirectory(outputDirectory);
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Create the MBOX reader using the required factory method and load options
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
            {
                // Read the first message to demonstrate loading before splitting
                MailMessage firstMessage = mboxReader.ReadNextMessage();
                if (firstMessage != null)
                {
                    Console.WriteLine($"First message subject: {firstMessage.Subject}");
                    firstMessage.Dispose();
                }
                else
                {
                    Console.WriteLine("No messages found in the MBOX file.");
                }

                // Define the maximum size (in bytes) for each split part (e.g., 10 MB)
                long maxPartSize = 10L * 1024L * 1024L; // 10 MB

                // Path pattern for split output files
                string splitOutputPath = Path.Combine(outputDirectory, "part.mbox");

                // Perform the split operation
                try
                {
                    mboxReader.SplitInto(maxPartSize, splitOutputPath);
                    Console.WriteLine("MBOX split operation completed successfully.");
                }
                catch (Exception splitEx)
                {
                    Console.Error.WriteLine($"Error during split operation: {splitEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
