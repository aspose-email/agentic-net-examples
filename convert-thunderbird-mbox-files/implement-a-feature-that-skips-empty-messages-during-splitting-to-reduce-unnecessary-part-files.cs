using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            const string inputPstPath = "input.pst";
            const string outputFolder = "output";
            const long chunkSize = 10 * 1024 * 1024; // 10 MB

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Guard input PST file existence; create minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                    {
                        // Create a predefined Inbox folder
                        placeholderPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Load the PST and split it into parts
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
                {
                    // Perform splitting (synchronous wait on async method)
                    pst.SplitIntoAsync(chunkSize, outputFolder, CancellationToken.None).Wait();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during PST splitting: {ex.Message}");
                return;
            }

            // After splitting, remove any empty part files
            try
            {
                string[] partFiles = Directory.GetFiles(outputFolder, "*_part*.pst");
                foreach (string partFile in partFiles)
                {
                    bool isEmpty = true;
                    try
                    {
                        using (PersonalStorage partPst = PersonalStorage.FromFile(partFile))
                        {
                            // If total items count is greater than zero, the part is not empty
                            if (partPst.Store.GetTotalItemsCount() > 0)
                            {
                                isEmpty = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to inspect part file '{partFile}': {ex.Message}");
                        // Treat as empty to avoid leaving corrupted files
                        isEmpty = true;
                    }

                    if (isEmpty)
                    {
                        try
                        {
                            File.Delete(partFile);
                            Console.WriteLine($"Deleted empty part file: {Path.GetFileName(partFile)}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete empty part file '{partFile}': {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while cleaning up empty parts: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
