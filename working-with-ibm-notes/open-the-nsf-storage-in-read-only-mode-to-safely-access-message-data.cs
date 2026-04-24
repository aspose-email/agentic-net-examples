using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

namespace AsposeEmailNsfExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string nsfPath = "sample.nsf";

                // Verify that the NSF file exists before attempting to open it.
                if (!File.Exists(nsfPath))
                {
                    Console.Error.WriteLine($"NSF file not found: {nsfPath}");
                    return;
                }

                // Open the NSF storage in read‑only mode.
                using (NotesStorageFacility notesStorage = new NotesStorageFacility(nsfPath))
                {
                    // Enumerate all messages contained in the NSF database.
                    foreach (MailMessage mail in notesStorage.EnumerateMessages())
                    {
                        using (mail)
                        {
                            Console.WriteLine($"Subject: {mail.Subject}");
                            Console.WriteLine($"From: {mail.From}");
                            Console.WriteLine($"Sent: {mail.Date}");
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
