using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputMsgPath = "input.msg";
            string outputMsgPath = "output_note.msg";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            MapiMessage loadedMessage = MapiMessage.Load(inputMsgPath);

            // Ensure the loaded message is a note type
            if (loadedMessage.SupportedType != MapiItemType.Note)
            {
                Console.Error.WriteLine("The provided MSG file is not a note.");
                return;
            }

            // Convert to MapiNote
            MapiNote note = (MapiNote)loadedMessage.ToMapiMessageItem();

            // ----- Begin placeholder for NotesDocument handling -----
            // The NotesDocument class is not part of the documented Aspose.Email API.
            // Replace the following placeholder with the actual NotesDocument implementation
            // when the appropriate API becomes available.
            // Example:
            // NotesDocument notesDoc = new NotesDocument();
            // notesDoc.Subject = note.Subject;
            // notesDoc.Body = note.Body;
            // notesDoc.Color = note.Color;
            // notesDoc.Save("output.nsf");
            // ---------------------------------------------------------

            // For demonstration, we will save the note back as a MSG file
            // Retrieve the underlying MapiMessage from the note
            MapiMessage underlyingMessage = note.GetUnderlyingMessage();

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Save the note as a MSG file
            underlyingMessage.Save(outputMsgPath);
            Console.WriteLine($"Note saved to {outputMsgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
