using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the PST file to be processed
            string pstFilePath = "sample.pst";

            // Path to the output text file that will contain the folder hierarchy
            string outputFilePath = "pst_hierarchy.txt";

            // Verify that the PST file exists before attempting to open it
            if (!File.Exists(pstFilePath))
            {
                Console.Error.WriteLine($"PST file not found: {pstFilePath}");
                return;
            }

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Open the PST file for reading
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Create a StreamWriter to write the hierarchy to the output file
                using (StreamWriter writer = new StreamWriter(outputFilePath))
                {
                    // Start writing from the root folder
                    WriteFolderHierarchy(pst.RootFolder, writer, 0);
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors to the error console
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively writes folder information with indentation to represent hierarchy
    static void WriteFolderHierarchy(FolderInfo folder, StreamWriter writer, int indentLevel)
    {
        // Build indentation string (2 spaces per level)
        string indent = new string(' ', indentLevel * 2);

        // Write the current folder's display name and item counts
        writer.WriteLine($"{indent}{folder.DisplayName} (Items: {folder.ContentCount}, Unread: {folder.ContentUnreadCount})");

        // Process each subfolder recursively
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            WriteFolderHierarchy(subFolder, writer, indentLevel + 1);
        }
    }
}
