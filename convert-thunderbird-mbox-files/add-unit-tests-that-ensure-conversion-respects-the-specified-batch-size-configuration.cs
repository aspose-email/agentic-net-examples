using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string mboxPath = "sample.mbox";
            string outputFolder = "split_output";
            long batchSize = 200; // bytes

            // Ensure output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Ensure MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                CreatePlaceholderMbox(mboxPath);
            }

            // Load the MBOX using Aspose.Email's MboxrdStorageReader
            using (MboxrdStorageReader reader = new MboxrdStorageReader(mboxPath, new MboxLoadOptions()))
            {
                // Perform split operation with specified batch size
                reader.SplitInto(batchSize, outputFolder);
            }

            // Verify that the split operation respected the batch size
            // Count the number of files created in the output folder
            string[] splitFiles = Directory.GetFiles(outputFolder, "*.mbox");
            int fileCount = splitFiles.Length;

            // Simple expectation: if total size > batchSize, expect at least 2 files
            long totalSize = new FileInfo(mboxPath).Length;
            int expectedMinFiles = totalSize > batchSize ? 2 : 1;

            if (fileCount >= expectedMinFiles)
            {
                Console.WriteLine($"Test Passed: {fileCount} split files created (expected at least {expectedMinFiles}).");
            }
            else
            {
                Console.Error.WriteLine($"Test Failed: Only {fileCount} split files created (expected at least {expectedMinFiles}).");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Creates a minimal MBOX file with three simple messages
    private static void CreatePlaceholderMbox(string path)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                for (int i = 1; i <= 3; i++)
                {
                    writer.WriteLine($"From - Sat Jan 01 00:00:0{i} 2022");
                    writer.WriteLine($"Subject: Test Message {i}");
                    writer.WriteLine("From: sender@example.com");
                    writer.WriteLine("To: recipient@example.com");
                    writer.WriteLine();
                    writer.WriteLine($"This is the body of test message {i}.");
                    writer.WriteLine();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
        }
    }
}
