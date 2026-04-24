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
            string filePath = "sample.nsf";

            // Verify that the file exists
            if (!File.Exists(filePath))
            {
                Console.Error.WriteLine($"File not found: {filePath}");
                return;
            }

            // Simple format check based on file extension
            if (!string.Equals(Path.GetExtension(filePath), ".nsf", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("The specified file is not an NSF database.");
                return;
            }

            // Attempt to open the NSF file
            try
            {
                using (NotesStorageFacility nsf = new NotesStorageFacility(filePath))
                {
                    Console.WriteLine("NSF file detected and opened successfully.");
                    // Example enumeration of messages (optional)
                    foreach (MailMessage message in nsf.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open NSF file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
