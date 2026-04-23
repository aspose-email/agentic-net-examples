using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
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

            const int maxRetryAttempts = 3;
            int attempt = 0;
            bool opened = false;
            NotesStorageFacility nsfFacility = null;

            // Retry logic for opening the NSF file.
            while (attempt < maxRetryAttempts && !opened)
            {
                try
                {
                    nsfFacility = new NotesStorageFacility(nsfPath);
                    opened = true;
                }
                catch (Exception ex)
                {
                    attempt++;
                    Console.Error.WriteLine($"Attempt {attempt} to open NSF file failed: {ex.Message}");
                    if (attempt < maxRetryAttempts)
                    {
                        // Wait before the next retry.
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.Error.WriteLine("Exceeded maximum retry attempts. Unable to open NSF file.");
                        return;
                    }
                }
            }

            // Use the opened NSF facility.
            using (nsfFacility)
            {
                // Enumerate messages contained in the NSF file.
                foreach (MailMessage mail in nsfFacility.EnumerateMessages())
                {
                    using (mail)
                    {
                        Console.WriteLine($"Subject: {mail.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
