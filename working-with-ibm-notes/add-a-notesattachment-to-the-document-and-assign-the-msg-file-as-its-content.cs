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
            string sourceMsgPath = "source.msg";

            // Ensure the source MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(sourceMsgPath))
            {
                MapiMessage placeholder = new MapiMessage(
                    "Placeholder",
                    "Placeholder body",
                    "sender@example.com",
                    "receiver@example.com");
                placeholder.Save(sourceMsgPath);
                Console.WriteLine($"Created placeholder MSG at {sourceMsgPath}");
            }

            // Load the MSG file that will become the attachment content
            using (MapiMessage attachedMessage = MapiMessage.Load(sourceMsgPath))
            {
                // Create a new MapiMessage document
                using (MapiMessage document = new MapiMessage(
                    "Document with NotesAttachment",
                    "Document body",
                    "author@example.com",
                    "recipient@example.com"))
                {
                    // Add the MSG as a NotesAttachment (embedded message)
                    document.Attachments.Add("NotesAttachment.msg", attachedMessage);

                    // Save the document containing the attachment
                    string outputPath = "document_with_notes.msg";
                    document.Save(outputPath);
                    Console.WriteLine($"Document saved with NotesAttachment at {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
