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
            string nsfPath = "sample.nsf";

            // Ensure the NSF file exists; create a minimal placeholder if it does not.
            if (!File.Exists(nsfPath))
            {
                try
                {
                    using (FileStream fs = File.Create(nsfPath))
                    {
                        // Placeholder file created; no content needed for enumeration demo.
                    }
                    Console.WriteLine($"Placeholder NSF file created at: {nsfPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder NSF file: {ex.Message}");
                    return;
                }
            }

            // Open the NSF storage for enumeration.
            using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
            {
                // Enumerate messages; each message is a MailMessage instance.
                foreach (MailMessage message in nsf.EnumerateMessages())
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
