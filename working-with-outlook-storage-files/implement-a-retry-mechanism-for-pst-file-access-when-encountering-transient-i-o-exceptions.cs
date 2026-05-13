using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";
            const int maxRetryAttempts = 3;
            const int retryDelayMilliseconds = 1000;

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file as a placeholder.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {createEx.Message}");
                    return;
                }
            }

            PersonalStorage pst = null;
            bool opened = false;

            for (int attempt = 1; attempt <= maxRetryAttempts && !opened; attempt++)
            {
                try
                {
                    pst = PersonalStorage.FromFile(pstPath);
                    opened = true;
                }
                catch (IOException ioEx)
                {
                    Console.Error.WriteLine($"I/O error on attempt {attempt}: {ioEx.Message}");
                    if (attempt == maxRetryAttempts)
                    {
                        Console.Error.WriteLine("All retry attempts exhausted. Exiting.");
                        return;
                    }
                    Thread.Sleep(retryDelayMilliseconds);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error while opening PST: {ex.Message}");
                    return;
                }
            }

            // Use the PST within a using block to ensure proper disposal.
            using (pst)
            {
                try
                {
                    int totalItemsCount = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total items in PST: {totalItemsCount}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
