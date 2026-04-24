using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            // Define NSF file paths to process
            List<string> nsfFiles = new List<string>
            {
                "sample1.nsf",
                "sample2.nsf"
            };

            foreach (string nsfPath in nsfFiles)
            {
                // Guard against missing file
                if (!File.Exists(nsfPath))
                {
                    Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                    continue;
                }

                try
                {
                    // Open the NSF storage facility
                    using (NotesStorageFacility client = new NotesStorageFacility(nsfPath))
                    {
                        int messageCount = 0;
                        foreach (MailMessage message in client.EnumerateMessages())
                        {
                            // Process each message as needed (placeholder)
                            messageCount++;
                        }

                        Console.WriteLine($"File: {nsfPath} - Total messages processed: {messageCount}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing NSF file '{nsfPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
