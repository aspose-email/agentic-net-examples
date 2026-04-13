using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the output file path for the MSG file
            string outputPath = "note.msg";

            // Ensure the directory for the output file exists
            string directoryPath = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            // Create a MapiNote with a subject and multiline body
            string subject = "Sample Note Subject";
            string body = "First line of the note.\r\nSecond line of the note.\r\nThird line of the note.";
            using (MapiNote note = new MapiNote(subject, body))
            {
                // Save the note as an MSG file
                note.Save(outputPath, NoteSaveFormat.Msg);
                Console.WriteLine($"MapiNote saved successfully to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
