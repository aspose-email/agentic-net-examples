using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";

            // Verify that the input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Counter for converted messages
            int convertedCount = 0;

            // Define a handler that increments the counter for each message processed
            MailStorageConverter.MailHandler handler = delegate (Aspose.Email.MailMessage message)
            {
                convertedCount++;
            };

            // Use a memory stream to avoid creating a PST file on disk
            using (MemoryStream pstStream = new MemoryStream())
            {
                // Perform the conversion; the PST data is written to the memory stream
                MailStorageConverter.MboxToPst(mboxPath, pstStream, handler);
            }

            // Display a summary of the conversion results
            Console.WriteLine($"Conversion completed. Total messages processed: {convertedCount}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
