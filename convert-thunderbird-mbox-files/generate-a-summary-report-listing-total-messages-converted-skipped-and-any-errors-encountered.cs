using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify MBOX file existence; create an empty placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // Empty MBOX placeholder created
                    }
                    Console.WriteLine($"Placeholder MBOX file created at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Counters for the conversion process
            int totalConverted = 0;
            int totalSkipped = 0;
            List<string> errorMessages = new List<string>();

            // Define the MailHandler delegate to process each message during conversion
            MailStorageConverter.MailHandler handler = delegate (MailMessage message)
            {
                try
                {
                    // Increment the converted counter for each successfully read message
                    totalConverted++;
                }
                catch (Exception ex)
                {
                    // Record any errors that occur while handling a message
                    errorMessages.Add($"Message handling error: {ex.Message}");
                }
            };

            // Perform the conversion inside a guarded block
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, handler))
                {
                    // Conversion succeeded; the PST is automatically saved to pstPath
                }
            }
            catch (Exception ex)
            {
                // Capture conversion-level errors
                errorMessages.Add($"Conversion error: {ex.Message}");
            }

            // Output the summary report
            Console.WriteLine("=== Conversion Summary ===");
            Console.WriteLine($"Total messages converted: {totalConverted}");
            Console.WriteLine($"Total messages skipped: {totalSkipped}");
            Console.WriteLine($"Errors encountered: {errorMessages.Count}");
            foreach (string err in errorMessages)
            {
                Console.WriteLine($"- {err}");
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
