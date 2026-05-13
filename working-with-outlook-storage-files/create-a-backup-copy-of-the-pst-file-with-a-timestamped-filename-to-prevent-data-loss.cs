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
            // Path to the original PST file
            string sourcePstPath = "sample.pst";

            // Verify source PST exists; create a minimal placeholder if missing
            if (!File.Exists(sourcePstPath))
            {
                try
                {
                    // Ensure the directory for the source PST exists
                    string sourceDir = Path.GetDirectoryName(sourcePstPath);
                    if (!string.IsNullOrEmpty(sourceDir) && !Directory.Exists(sourceDir))
                    {
                        Directory.CreateDirectory(sourceDir);
                    }

                    // Create an empty Unicode PST as a placeholder
                    PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{sourcePstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Build a timestamped backup file name
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string backupFileName = $"{Path.GetFileNameWithoutExtension(sourcePstPath)}_{timestamp}{Path.GetExtension(sourcePstPath)}";
            string backupPstPath = Path.Combine(Path.GetDirectoryName(sourcePstPath) ?? string.Empty, backupFileName);

            // Ensure the backup directory exists
            try
            {
                string backupDir = Path.GetDirectoryName(backupPstPath);
                if (!string.IsNullOrEmpty(backupDir) && !Directory.Exists(backupDir))
                {
                    Directory.CreateDirectory(backupDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to ensure backup directory: {ex.Message}");
                return;
            }

            // Open the original PST and save a copy with the timestamped name
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(sourcePstPath))
                {
                    pst.SaveAs(backupPstPath, FileFormat.Pst);
                }

                Console.WriteLine($"Backup created successfully at '{backupPstPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during PST backup: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
