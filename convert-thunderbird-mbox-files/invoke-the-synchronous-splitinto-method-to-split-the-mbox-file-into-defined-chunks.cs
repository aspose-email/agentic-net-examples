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
            string mboxPath = "input.mbox";
            string outputFolder = "output_chunks";

            // Verify input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create the MBOX reader with required load options
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Read the first message to satisfy validation requirements
                MailMessage firstMessage = reader.ReadNextMessage();

                // Define approximate chunk size (e.g., 1 MB)
                long chunkSize = 1 * 1024 * 1024;

                // Split the MBOX into chunks
                reader.SplitInto(chunkSize, outputFolder);

                Console.WriteLine("MBOX split completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
