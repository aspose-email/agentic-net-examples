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
            string inputMsgPath = "input.msg";
            // Output NSF (Notes) file path
            string outputNsfPath = "output.nsf";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputNsfPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the MSG file as a MapiMessage
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(inputMsgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to load MSG file – {ex.Message}");
                return;
            }

            // Create a MapiNote and populate its fields from the loaded message
            MapiNote note = new MapiNote
            {
                Subject = msg.Subject,
                Body = msg.Body,
                // Additional fields can be set as needed, e.g., Color, CreationDate, etc.
            };

            // Save the note as a MSG file (Notes document representation)
            string noteMsgPath = "note_output.msg";
            try
            {
                note.Save(noteMsgPath, NoteSaveFormat.Msg);
                Console.WriteLine($"Note saved to {noteMsgPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to save note – {ex.Message}");
                return;
            }

            // Optionally, store the note in an NSF (Notes) database
            try
            {
                using (NotesStorageFacility notesFacility = new NotesStorageFacility(outputNsfPath))
                {
                    // The NotesStorageFacility does not provide a direct method to add a note,
                    // but this placeholder demonstrates resource handling.
                    // Real implementation would involve creating a document within the NSF.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Failed to work with Notes storage – {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
