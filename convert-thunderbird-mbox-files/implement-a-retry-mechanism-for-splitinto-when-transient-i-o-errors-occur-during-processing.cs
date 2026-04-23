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
            const string outputFolder = "output";

            // Ensure the PST file exists; create a minimal one if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Minimal PST created; no additional content needed for this example.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputFolder))
            {
                try
                {
                    Directory.CreateDirectory(outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                const long chunkSize = 5 * 1024 * 1024; // 5 MB per chunk (adjust as needed)
                const int maxRetries = 3;
                int attempt = 0;
                bool splitSucceeded = false;

                while (attempt < maxRetries && !splitSucceeded)
                {
                    try
                    {
                        pst.SplitInto(chunkSize, outputFolder);
                        splitSucceeded = true;
                        Console.WriteLine("PST split completed successfully.");
                    }
                    catch (IOException ioEx)
                    {
                        attempt++;
                        Console.Error.WriteLine($"I/O error during SplitInto (attempt {attempt}): {ioEx.Message}");
                        if (attempt >= maxRetries)
                        {
                            Console.Error.WriteLine("Maximum retry attempts reached. Operation aborted.");
                            return;
                        }
                        Thread.Sleep(1000); // Wait before retrying.
                    }
                    catch (AsposeException aspEx)
                    {
                        attempt++;
                        Console.Error.WriteLine($"Aspose error during SplitInto (attempt {attempt}): {aspEx.Message}");
                        if (attempt >= maxRetries)
                        {
                            Console.Error.WriteLine("Maximum retry attempts reached. Operation aborted.");
                            return;
                        }
                        Thread.Sleep(1000);
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
