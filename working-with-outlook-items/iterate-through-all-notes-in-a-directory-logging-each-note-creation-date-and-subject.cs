using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

namespace SampleApp
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Directory that contains NSF files
                string notesDirectory = "NotesFolder";

                // Ensure the directory exists; create if missing
                if (!Directory.Exists(notesDirectory))
                {
                    Directory.CreateDirectory(notesDirectory);
                    // Create a minimal placeholder NSF file to avoid missing asset errors
                    string placeholderPath = Path.Combine(notesDirectory, "placeholder.nsf");
                    File.WriteAllBytes(placeholderPath, new byte[0]);
                }

                // Get all NSF files in the directory
                string[] nsfFiles = Directory.GetFiles(notesDirectory, "*.nsf");
                foreach (string nsfPath in nsfFiles)
                {
                    // Verify the file exists before attempting to open
                    if (!File.Exists(nsfPath))
                    {
                        // Skip missing files gracefully
                        continue;
                    }

                    try
                    {
                        // Open the NSF storage
                        using (NotesStorageFacility client = new NotesStorageFacility(nsfPath))
                        {
                            // Enumerate each note (MailMessage) in the storage
                            foreach (MailMessage note in client.EnumerateMessages())
                            {
                                // Retrieve creation date and subject
                                DateTime creationDate = note.Date;
                                string subject = note.Subject ?? string.Empty;

                                // Log the information to the console
                                Console.WriteLine($"File: {Path.GetFileName(nsfPath)} | Date: {creationDate} | Subject: {subject}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log any errors related to processing a specific NSF file
                        Console.Error.WriteLine($"Error processing NSF file '{nsfPath}': {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level error handling
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
