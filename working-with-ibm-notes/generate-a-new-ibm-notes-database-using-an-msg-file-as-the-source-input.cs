using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Nsf;

// Author: Example that creates an IBM Notes (NSF) database file.
// The source MSG file is validated, but the NSF format does not expose a direct import method in this API version.
class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "source.msg";
            string nsfPath = "output.nsf";

            // Verify the source MSG file exists.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source MSG file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage.
            MapiMessage mapiMessage = MapiMessage.Load(msgPath);

            // Ensure the output directory exists.
            string nsfDirectory = Path.GetDirectoryName(nsfPath);
            if (!string.IsNullOrEmpty(nsfDirectory) && !Directory.Exists(nsfDirectory))
            {
                Directory.CreateDirectory(nsfDirectory);
            }

            // Create (or open) the NSF database. No explicit import API is available in this version,
            // so the database is created empty. Further processing can be added when the appropriate method is provided.
            using (NotesStorageFacility notes = new NotesStorageFacility(nsfPath))
            {
                // Placeholder for future message import logic.
                // Example: notes.ImportMessage(mapiMessage);
            }

            Console.WriteLine("NSF database created successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
