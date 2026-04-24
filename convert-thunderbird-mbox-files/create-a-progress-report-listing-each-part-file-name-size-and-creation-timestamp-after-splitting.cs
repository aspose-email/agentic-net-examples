using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Input PST file path
            string pstPath = "storage.pst";
            // Output folder for split parts
            string outputFolder = "SplitParts";

            // Ensure input PST exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
                    return;
                }
            }

            // Define chunk size (e.g., 10 MB)
            long chunkSize = 10 * 1024 * 1024;

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Split the PST asynchronously
                try
                {
                    await pst.SplitIntoAsync(chunkSize, outputFolder, CancellationToken.None);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during PST split: {ex.Message}");
                    return;
                }
            }

            // After splitting, generate progress report
            try
            {
                string[] partFiles = Directory.GetFiles(outputFolder, "*_part*.pst");
                Console.WriteLine("Split parts report:");
                foreach (string partFilePath in partFiles)
                {
                    FileInfo info = new FileInfo(partFilePath);
                    string fileName = info.Name;
                    long fileSize = info.Length;
                    DateTime creationTime = info.CreationTime;
                    Console.WriteLine($"File: {fileName}, Size: {fileSize} bytes, Created: {creationTime}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to generate report: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
