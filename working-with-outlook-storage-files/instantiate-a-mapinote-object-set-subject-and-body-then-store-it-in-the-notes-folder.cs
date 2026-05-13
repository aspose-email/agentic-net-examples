using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path
            string pstPath = "NotesSample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Notes folder (create if it does not exist)
                FolderInfo notesFolder;
                try
                {
                    notesFolder = pst.GetPredefinedFolder(StandardIpmFolder.Notes);
                }
                catch (Exception)
                {
                    // If the predefined Notes folder is not present, create it
                    notesFolder = pst.RootFolder.AddSubFolder("Notes");
                }

                // Create a MapiNote with subject and body
                MapiNote note = new MapiNote
                {
                    Subject = "Sample Note",
                    Body = "This is the body of the note."
                };

                // Add the note to the Notes folder
                try
                {
                    string entryId = notesFolder.AddMapiMessageItem(note);
                    Console.WriteLine($"Note added with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add note to folder: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
