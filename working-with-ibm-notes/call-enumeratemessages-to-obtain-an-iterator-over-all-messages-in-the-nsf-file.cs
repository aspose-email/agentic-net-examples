using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

namespace AsposeEmailNSFExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the NSF file.
                string nsfPath = "sample.nsf";

                // Guard against missing file.
                if (!File.Exists(nsfPath))
                {
                    Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                    return;
                }

                // Open the NSF storage facility.
                using (NotesStorageFacility facility = new NotesStorageFacility(nsfPath))
                {
                    // Enumerate all messages in the NSF file.
                    foreach (MailMessage message in facility.EnumerateMessages())
                    {
                        // Dispose each MailMessage after processing.
                        using (message)
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"Sent: {message.Date}");
                            Console.WriteLine(new string('-', 40));
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
}
