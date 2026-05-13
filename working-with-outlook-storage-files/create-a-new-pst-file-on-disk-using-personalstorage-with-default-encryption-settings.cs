using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the PST file path
            string pstPath = "output.pst";

            // Ensure the directory for the PST file exists
            string directory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // If a file already exists at the target location, delete it to allow creation
            if (File.Exists(pstPath))
            {
                Console.WriteLine($"File \"{pstPath}\" already exists and will be overwritten.");
                File.Delete(pstPath);
            }

            // Create a new PST file with default (Unicode) format
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // PST created; no additional actions required for this example
            }

            Console.WriteLine($"PST file successfully created at \"{pstPath}\".");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
