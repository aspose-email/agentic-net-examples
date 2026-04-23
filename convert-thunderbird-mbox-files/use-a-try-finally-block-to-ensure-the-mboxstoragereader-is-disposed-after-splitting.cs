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
            string mboxPath = "input.mbox";
            string outputFolder = "SplitParts";
            long partSize = 10_000_000; // 10 MB per part

            // Guard input file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output folder exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder: {ex.Message}");
                return;
            }

            // Create the reader with required options
            MboxStorageReader reader = null;
            try
            {
                reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

                // Example of reading messages (optional)
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    // Process each message if needed
                    // For this sample we just continue to the split operation
                }

                // Split the MBOX into smaller parts
                try
                {
                    reader.SplitInto(partSize, outputFolder);
                    Console.WriteLine($"MBOX split completed. Parts are stored in '{outputFolder}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during split operation: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or use MboxStorageReader: {ex.Message}");
                return;
            }
            finally
            {
                // Ensure the reader is disposed
                if (reader != null)
                {
                    reader.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
