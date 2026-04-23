using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

class Program
{
    static void Main()
    {
        try
        {
            // Define the directory containing part files and the manifest file name
            string partsDirectory = Directory.GetCurrentDirectory();
            string manifestPath = Path.Combine(partsDirectory, "manifest.txt");

            // Verify that the directory exists
            if (!Directory.Exists(partsDirectory))
            {
                Console.Error.WriteLine($"Directory does not exist: {partsDirectory}");
                return;
            }

            // Retrieve all files in the directory (excluding the manifest itself if it already exists)
            string[] filePaths;
            try
            {
                filePaths = Directory.GetFiles(partsDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            // Prepare a StringBuilder to accumulate manifest lines
            StringBuilder manifestBuilder = new StringBuilder();

            foreach (string filePath in filePaths)
            {
                // Skip the manifest file itself to avoid self-referencing
                if (string.Equals(filePath, manifestPath, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Ensure the file exists before processing
                if (!File.Exists(filePath))
                {
                    Console.Error.WriteLine($"File not found, skipping: {filePath}");
                    continue;
                }

                // Compute SHA256 checksum
                try
                {
                    using (FileStream fileStream = File.OpenRead(filePath))
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        byte[] hashBytes = sha256.ComputeHash(fileStream);
                        StringBuilder hashStringBuilder = new StringBuilder(hashBytes.Length * 2);
                        foreach (byte b in hashBytes)
                        {
                            hashStringBuilder.Append(b.ToString("x2"));
                        }

                        string fileName = Path.GetFileName(filePath);
                        string checksum = hashStringBuilder.ToString();
                        manifestBuilder.AppendLine($"{fileName} {checksum}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                    // Continue with next file
                }
            }

            // Write the manifest file
            try
            {
                using (StreamWriter writer = new StreamWriter(manifestPath, false, Encoding.UTF8))
                {
                    writer.Write(manifestBuilder.ToString());
                }
                Console.WriteLine($"Manifest created at: {manifestPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write manifest file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
