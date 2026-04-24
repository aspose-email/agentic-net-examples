using System;
using System.IO;
using System.Threading;
using Aspose.Email;
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
            string directory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{directory}': {dirEx.Message}");
                    return;
                }
            }

            // Retry parameters
            const int maxAttempts = 3;
            const int delayMilliseconds = 1000;

            // Attempt to create the PST file with retry logic for transient I/O errors
            for (int attempt = 1; attempt <= maxAttempts; attempt++)
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        // Example: add a predefined folder (Inbox) to the PST
                        pst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                        Console.WriteLine($"PST file created successfully at '{pstFilePath}'.");
                    }

                    // Creation succeeded, exit the retry loop
                    break;
                }
                catch (IOException ioEx)
                {
                    // Transient I/O error – retry unless this was the last attempt
                    Console.Error.WriteLine($"I/O error on attempt {attempt}: {ioEx.Message}");
                    if (attempt == maxAttempts)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. PST creation failed.");
                        return;
                    }
                    Thread.Sleep(delayMilliseconds);
                }
                catch (AsposeException aspEx)
                {
                    // Non-transient Aspose-specific error – report and abort
                    Console.Error.WriteLine($"Aspose error: {aspEx.Message}");
                    return;
                }
                catch (Exception ex)
                {
                    // Unexpected error – report and abort
                    Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception outerEx)
        {
            Console.Error.WriteLine($"Unhandled exception: {outerEx.Message}");
        }
    }
}
