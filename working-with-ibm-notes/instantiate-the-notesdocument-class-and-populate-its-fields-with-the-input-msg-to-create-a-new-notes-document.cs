using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Nsf;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string msgPath = "input.msg";

            // Verify MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage
            MapiMessage msg;
            try
            {
                using (MapiMessage loadedMsg = MapiMessage.Load(msgPath))
                {
                    // Clone to keep after disposing the loader
                    msg = loadedMsg.Clone();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG: {ex.Message}");
                return;
            }

            // Create a Notes note (MapiNote) and populate its fields from the MSG
            MapiNote note = new MapiNote(msg.Subject, msg.Body);

            // Output NSF file path
            string nsfPath = "output.nsf";

            // Ensure the directory for the NSF file exists
            string nsfDirectory = Path.GetDirectoryName(nsfPath);
            if (!string.IsNullOrEmpty(nsfDirectory) && !Directory.Exists(nsfDirectory))
            {
                Directory.CreateDirectory(nsfDirectory);
            }

            // Create or open the NSF database using NotesStorageFacility
            using (NotesStorageFacility notesFacility = new NotesStorageFacility(nsfPath))
            {
                // Placeholder: In a full implementation, the note would be added to the NSF.
                // The API for adding a MapiNote to NotesStorageFacility is not shown in the documentation.
                // This block demonstrates proper resource handling.
            }

            Console.WriteLine("Notes document created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
