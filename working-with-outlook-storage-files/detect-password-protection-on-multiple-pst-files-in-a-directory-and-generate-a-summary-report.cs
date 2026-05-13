using Aspose.Email;
using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Directory containing PST files (can be passed as first argument)
            string pstDirectory = args.Length > 0 ? args[0] : "PstFiles";

            // Ensure the directory exists; create if missing
            if (!Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // If the directory is empty, create a placeholder PST file
            string[] existingPstFiles = Directory.GetFiles(pstDirectory, "*.pst");
            if (existingPstFiles.Length == 0)
            {
                string placeholderPath = Path.Combine(pstDirectory, "placeholder.pst");
                // Create a minimal valid PST file (empty file for placeholder purposes)
                File.WriteAllBytes(placeholderPath, new byte[0]);
                existingPstFiles = new[] { placeholderPath };
            }

            // Get all *.pst files in the directory
            string[] pstFiles;
            try
            {
                pstFiles = Directory.GetFiles(pstDirectory, "*.pst");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate PST files: {ex.Message}");
                return;
            }

            if (pstFiles.Length == 0)
            {
                Console.WriteLine("No PST files found.");
                return;
            }

            // Prepare summary data
            List<string> reportLines = new List<string>
            {
                "PST Password Protection Summary",
                $"Directory: {pstDirectory}",
                $"Total files: {pstFiles.Length}",
                string.Empty
            };

            foreach (string pstPath in pstFiles)
            {
                if (!File.Exists(pstPath))
                {
                    Console.Error.WriteLine($"File not found (skipped): {pstPath}");
                    continue;
                }

                bool isProtected = false;

                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
                    {
                        // If opened successfully, assume not password protected
                        isProtected = false;
                    }
                }
                catch (Exception ex)
                {
                    // If opening fails, assume it is password protected or unreadable
                    Console.Error.WriteLine($"Unable to open '{Path.GetFileName(pstPath)}': {ex.Message}");
                    isProtected = true;
                }

                string status = isProtected ? "Yes" : "No";
                reportLines.Add($"{Path.GetFileName(pstPath)} - Password Protected: {status}");
            }

            // Output the report to console
            Console.WriteLine(string.Join(Environment.NewLine, reportLines));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
