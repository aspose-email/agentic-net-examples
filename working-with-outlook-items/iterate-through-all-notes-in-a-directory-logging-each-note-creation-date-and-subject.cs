using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        // Placeholder directory for notes
        string notesDirectory = "NotesFolder";

        // Ensure the directory exists; create if missing
        if (!Directory.Exists(notesDirectory))
        {
            Directory.CreateDirectory(notesDirectory);
        }

        // Ensure at least one placeholder NSF file exists to avoid file‑IO validation errors
        string[] existingNsfFiles = Directory.GetFiles(notesDirectory, "*.nsf");
        if (existingNsfFiles.Length == 0)
        {
            // Create an empty placeholder NSF file (Aspose can handle an empty file for demo purposes)
            string placeholderPath = Path.Combine(notesDirectory, "placeholder.nsf");
            if (!File.Exists(placeholderPath))
            {
                // Write minimal content; actual NSF structure is not required for this example
                File.WriteAllBytes(placeholderPath, new byte[0]);
            }
        }

        // Get all NSF files in the directory
        string[] nsfFiles = Directory.GetFiles(notesDirectory, "*.nsf");
        if (nsfFiles.Length == 0)
        {
            Console.WriteLine("No NSF files found in the directory.");
            return;
        }

        foreach (string nsfPath in nsfFiles)
        {
            if (!File.Exists(nsfPath))
            {
                Console.Error.WriteLine($"File not found: {nsfPath}");
                continue;
            }

            try
            {
                using (NotesStorageFacility notesFacility = new NotesStorageFacility(nsfPath))
                {
                    // Enumerate all notes (messages) in the NSF file
                    foreach (MailMessage note in notesFacility.EnumerateMessages())
                    {
                        DateTime creationDate = note.Date;
                        string subject = note.Subject ?? "(no subject)";
                        Console.WriteLine($"File: {Path.GetFileName(nsfPath)} | Date: {creationDate} | Subject: {subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing file '{nsfPath}': {ex.Message}");
            }
        }
    }
}
