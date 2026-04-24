using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the NSF file
            string nsfPath = "sample.nsf";

            // Verify that the NSF file exists before attempting to open it
            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                return;
            }

            // Maximum number of messages to process
            const int maxMessages = 100;
            int processedCount = 0;

            // Open the NSF storage facility
            using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
            {
                // Enumerate messages safely
                foreach (MailMessage message in nsf.EnumerateMessages())
                {
                    // Process the message (example: output subject)
                    Console.WriteLine($"Subject: {message.Subject}");

                    processedCount++;
                    if (processedCount >= maxMessages)
                    {
                        Console.WriteLine($"Reached the processing limit of {maxMessages} messages.");
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
