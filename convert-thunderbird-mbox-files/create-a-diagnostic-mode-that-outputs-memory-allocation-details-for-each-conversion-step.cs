using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Display memory before conversion
            long memoryBefore = GC.GetTotalMemory(forceFullCollection: false);
            Console.WriteLine($"Memory before conversion: {memoryBefore} bytes");

            // Define the mail handler to output diagnostic info for each message
            MailStorageConverter.MailHandler handler = delegate (MailMessage message)
            {
                Console.WriteLine($"Processing message: Subject=\"{message.Subject}\"");

                // Approximate size of the message body (if present)
                int bodyLength = message.Body != null ? message.Body.Length : 0;
                Console.WriteLine($"Body length: {bodyLength} characters");

                // Current memory usage after handling this message
                long currentMemory = GC.GetTotalMemory(forceFullCollection: false);
                Console.WriteLine($"Current memory usage: {currentMemory} bytes");
                Console.WriteLine(new string('-', 40));
            };

            // Perform the conversion with the diagnostic handler
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, handler))
            {
                // Optionally, you could work with the resulting PST here
                Console.WriteLine("Conversion completed successfully.");
            }

            // Display memory after conversion
            long memoryAfter = GC.GetTotalMemory(forceFullCollection: false);
            Console.WriteLine($"Memory after conversion: {memoryAfter} bytes");
            Console.WriteLine($"Memory delta: {memoryAfter - memoryBefore} bytes");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
