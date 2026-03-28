using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "output.pst";
            string fileToAdd = "sample.txt";

            // Ensure the file to add exists; create a minimal placeholder if missing
            if (!File.Exists(fileToAdd))
            {
                try
                {
                    File.WriteAllText(fileToAdd, "Placeholder content");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder file: {ex.Message}");
                    return;
                }
            }

            // Open existing PST for writing or create a new one if it does not exist
            PersonalStorage pst = null;
            try
            {
                if (File.Exists(pstPath))
                {
                    pst = PersonalStorage.FromFile(pstPath, true);
                }
                else
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error opening or creating PST file: {ex.Message}");
                return;
            }

            using (pst)
            {
                // Add the file into the root folder of the PST
                try
                {
                    string entryId = pst.RootFolder.AddFile(fileToAdd, "IPM.Note");
                    Console.WriteLine($"File added to PST. EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error adding file to PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
