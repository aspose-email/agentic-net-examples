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
            // Path to the existing MSG file that will be attached as a note
            string noteFilePath = "note.msg";

            // Verify the source MSG file exists
            if (!File.Exists(noteFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(noteFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source MSG file not found: {noteFilePath}");
                return;
            }

            // Load the MSG file to be used as the attachment content
            MapiMessage noteMessage = MapiMessage.Load(noteFilePath);

            // Create a new MAPI message that will hold the notes attachment
            MapiMessage mainMessage = new MapiMessage
            {
                Subject = "Message with NotesAttachment",
                Body = "This message contains a notes attachment."
            };

            // Add the loaded MSG as an embedded attachment (NotesAttachment)
            // The attachment name can be any desired string; here we use the original file name
            mainMessage.Attachments.Add("NoteAttachment.msg", noteMessage);

            // Define output path for the resulting MSG file
            string outputFilePath = "MessageWithNotes.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Save the composed message with the notes attachment
            mainMessage.Save(outputFilePath);
            Console.WriteLine($"Message saved successfully: {outputFilePath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
