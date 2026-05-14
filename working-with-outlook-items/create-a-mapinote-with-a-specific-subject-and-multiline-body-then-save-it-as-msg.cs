using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path for the MSG file
            string outputPath = "note.msg";

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MapiNote with a subject and a multiline body
            string subject = "Meeting Reminder";
            string body = "Dear Team,\nPlease attend the meeting at 10 AM.\nRegards,\nManager";

            using (MapiNote note = new MapiNote(subject, body))
            {
                // Save the note as an MSG file
                note.Save(outputPath, NoteSaveFormat.Msg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
