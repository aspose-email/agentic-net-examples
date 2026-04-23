using System;
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

            // Guard against missing file
            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"NSF file not found at path: {nsfPath}");
                return;
            }

            // Open the NSF storage facility
            using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
            {
                // Enumerate messages in the NSF database
                foreach (MailMessage mailMessage in nsf.EnumerateMessages())
                {
                    // Ensure each MailMessage is disposed after use
                    using (mailMessage)
                    {
                        Console.WriteLine($"Subject: {mailMessage.Subject}");
                    }
                }
            }

            // Limitations note:
            // NSF files created with Lotus Notes versions older than 7 may:
            // - Lack support for certain rich text features, causing loss of formatting.
            // - Miss newer MIME conversion flags, leading to incomplete message bodies.
            // - Contain legacy attachment structures that Aspose.Email may not fully parse.
            // - Throw NotSupportedException when accessing properties introduced in later versions.
            // When working with such files, expect possible missing data or exceptions during enumeration.
        }
        catch (AsposeException ex)
        {
            Console.Error.WriteLine($"Aspose.Email error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
