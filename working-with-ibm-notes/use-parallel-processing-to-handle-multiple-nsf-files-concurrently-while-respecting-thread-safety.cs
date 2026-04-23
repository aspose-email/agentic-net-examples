using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            // List of NSF files to process
            List<string> nsfFiles = new List<string>
            {
                "sample1.nsf",
                "sample2.nsf"
            };

            // Process each NSF file in parallel
            Parallel.ForEach(nsfFiles, nsfPath =>
            {
                // Verify the NSF file exists
                if (!File.Exists(nsfPath))
                {
                    Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                    return;
                }

                try
                {
                    // Open the NSF file
                    using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
                    {
                        // Enumerate all messages in the NSF
                        foreach (MailMessage message in nsf.EnumerateMessages())
                        {
                            // Example processing: output the subject
                            Console.WriteLine($"File: {nsfPath}, Subject: {message.Subject}");
                            // Dispose the message after use
                            message.Dispose();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing {nsfPath}: {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
