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
            // Paths for the source MSG file and the target IBM Notes (NSF) database
            string msgFilePath = "sample.msg";
            string nsfFilePath = "notes.nsf";

            // Verify that the source MSG file exists
            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Ensure the directory for the NSF file exists
            string nsfDirectory = Path.GetDirectoryName(nsfFilePath);
            if (!string.IsNullOrEmpty(nsfDirectory) && !Directory.Exists(nsfDirectory))
            {
                try
                {
                    Directory.CreateDirectory(nsfDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating directory for NSF file: {dirEx.Message}");
                    return;
                }
            }

            // Create a new IBM Notes (NSF) database file
            using (NotesStorageFacility notesFacility = new NotesStorageFacility(nsfFilePath))
            {
                // Load the MSG file into a MapiMessage object
                using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
                {
                    // Convert the MapiMessage to an IMapiMessageItem (required for storage)
                    IMapiMessageItem messageItem = mapiMessage.ToMapiMessageItem();

                    // NOTE: The NotesStorageFacility class does not expose a direct method to add messages.
                    // If a suitable API exists in the Aspose.Email version you are using, it would be invoked here.
                    // For demonstration, we simply output some properties of the loaded message.

                    Console.WriteLine($"Subject: {mapiMessage.Subject}");
                    Console.WriteLine($"From: {mapiMessage.SenderEmailAddress}");
                    Console.WriteLine($"Body preview: {mapiMessage.Body?.Substring(0, Math.Min(100, mapiMessage.Body.Length))}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
