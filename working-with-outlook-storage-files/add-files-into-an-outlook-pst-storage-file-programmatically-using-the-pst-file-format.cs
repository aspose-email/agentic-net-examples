using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path
            string pstFilePath = "output.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Create or open the PST file
            PersonalStorage pst;
            if (File.Exists(pstFilePath))
            {
                // Open existing PST for writing
                pst = PersonalStorage.FromFile(pstFilePath, true);
            }
            else
            {
                // Create a new Unicode PST file
                pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode);
            }

            using (pst)
            {
                // List of files to add to the PST
                List<string> filesToAdd = new List<string>
                {
                    "sample1.txt",
                    "sample2.pdf"
                };

                // Ensure each input file exists; create a tiny placeholder if missing
                foreach (string filePath in filesToAdd)
                {
                    if (!File.Exists(filePath))
                    {
                        try
                        {
                            // Create a minimal placeholder file
                            using (FileStream placeholder = File.Create(filePath))
                            {
                                byte[] content = System.Text.Encoding.UTF8.GetBytes("Placeholder content");
                                placeholder.Write(content, 0, content.Length);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error creating placeholder for '{filePath}': {ex.Message}");
                            continue;
                        }
                    }

                    try
                    {
                        // Add the file to the root folder of the PST as a standard mail item
                        string entryId = pst.RootFolder.AddFile(filePath, "IPM.Note");
                        Console.WriteLine($"Added '{filePath}' to PST. EntryId: {entryId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error adding file '{filePath}' to PST: {ex.Message}");
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
