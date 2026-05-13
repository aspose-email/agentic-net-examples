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
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {createEx.Message}");
                    return;
                }
            }

            // Open PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Target folder name
                const string targetFolderName = "MyNotes";

                FolderInfo targetFolder;

                // Try to get the folder; if it does not exist, create it
                try
                {
                    targetFolder = pst.RootFolder.GetSubFolder(targetFolderName);
                }
                catch (Exception getFolderEx)
                {
                    Console.Error.WriteLine($"Folder '{targetFolderName}' not found: {getFolderEx.Message}");
                    try
                    {
                        targetFolder = pst.RootFolder.AddSubFolder(targetFolderName);
                        Console.WriteLine($"Created folder '{targetFolderName}'.");
                    }
                    catch (Exception addFolderEx)
                    {
                        Console.Error.WriteLine($"Failed to create folder '{targetFolderName}': {addFolderEx.Message}");
                        return;
                    }
                }

                // Create a MapiNote
                using (MapiNote note = new MapiNote())
                {
                    note.Subject = "Sample Note";
                    note.Body = "This is a sample sticky note created via Aspose.Email.";
                    note.Color = NoteColor.Yellow;

                    // Attempt to add the note to the folder and capture any exception
                    try
                    {
                        string entryId = targetFolder.AddMapiMessageItem(note);
                        Console.WriteLine($"Note added successfully. EntryId: {entryId}");
                    }
                    catch (Exception addNoteEx)
                    {
                        Console.Error.WriteLine($"Error adding note to folder '{targetFolderName}': {addNoteEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
