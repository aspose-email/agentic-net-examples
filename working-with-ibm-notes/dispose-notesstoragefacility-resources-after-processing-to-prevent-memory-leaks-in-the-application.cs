using System;
using System.IO;
using Aspose.Email.Storage.Nsf;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string nsfPath = "sample.nsf";

            // Ensure the NSF file exists; create a minimal placeholder if missing
            if (!File.Exists(nsfPath))
            {
                try
                {
                    using (FileStream placeholder = File.Create(nsfPath))
                    {
                        // No content needed for an empty NSF placeholder
                    }
                }
                catch (Exception fileEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder NSF file: {fileEx.Message}");
                    return;
                }
            }

            // Open the NSF storage facility and enumerate messages
            try
            {
                using (NotesStorageFacility nsf = new NotesStorageFacility(nsfPath))
                {
                    foreach (MailMessage message in nsf.EnumerateMessages())
                    {
                        // Process each message (example: output subject)
                        Console.WriteLine($"Subject: {message.Subject}");
                        // Dispose the message after use
                        message.Dispose();
                    }
                }
            }
            catch (Exception nsfEx)
            {
                Console.Error.WriteLine($"Error processing NSF file: {nsfEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
