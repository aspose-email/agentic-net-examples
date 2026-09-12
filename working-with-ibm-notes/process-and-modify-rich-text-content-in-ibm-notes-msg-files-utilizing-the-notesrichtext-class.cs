using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

// Author: Generated example for processing IBM Notes rich text in MSG files using Aspose.Email
class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "note.msg";
            string outputPath = "modified_note.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Load the Outlook MSG file
            MapiMessage msg = MapiMessage.Load(inputPath);

            // Ensure the message is a Notes note
            if (msg.SupportedType == MapiItemType.Note)
            {
                // Convert to MapiNote for richer note-specific API
                MapiNote note = (MapiNote)msg.ToMapiMessageItem();

                // ------------------------------------------------------------
                // Process and modify rich text content using NotesRichText.
                // The NotesRichText class is part of Aspose.Email for handling
                // IBM Notes rich‑text. Replace the placeholder code below with
                // actual NotesRichText manipulation as per the library docs.
                // ------------------------------------------------------------
                // NotesRichText richText = note.NotesRichText;
                // richText.AppendText("Added by Aspose.Email");
                // note.SetNotesRichText(richText);
                // ------------------------------------------------------------

                // Retrieve the underlying MapiMessage after modifications
                MapiMessage modifiedMsg = note.GetUnderlyingMessage();

                // Save the modified message back to disk
                modifiedMsg.Save(outputPath);
                Console.WriteLine($"Modified note saved to {outputPath}");
            }
            else
            {
                Console.Error.WriteLine("The loaded MSG file is not a Notes note.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
