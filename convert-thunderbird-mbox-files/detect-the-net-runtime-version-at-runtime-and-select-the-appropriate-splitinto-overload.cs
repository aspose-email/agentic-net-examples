using System;
using System.IO;
using System.Runtime.InteropServices;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define PST file path and output folder for split parts
            string pstPath = "sample.pst";
            string outputFolder = "SplitParts";

            // Ensure the output folder exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Guard PST file existence; create a minimal placeholder if missing
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

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Determine .NET runtime version
                Version runtimeVersion = Environment.Version;
                // For .NET Core/.NET 5+ RuntimeInformation can provide more detail
                string frameworkDesc = RuntimeInformation.FrameworkDescription;

                Console.WriteLine($"Running on {frameworkDesc} (Version {runtimeVersion})");

                // Define chunk size (e.g., 10 MB)
                long chunkSize = 10L * 1024 * 1024; // 10 MB

                // Choose overload based on runtime version
                // Example: use overload with prefix for .NET 6 or higher
                if (runtimeVersion.Major >= 6)
                {
                    string prefix = "Part";
                    try
                    {
                        pst.SplitInto(chunkSize, prefix, outputFolder);
                        Console.WriteLine("PST split using overload with prefix completed.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during SplitInto with prefix: {ex.Message}");
                    }
                }
                else
                {
                    // Use overload without prefix for older runtimes
                    try
                    {
                        pst.SplitInto(chunkSize, outputFolder);
                        Console.WriteLine("PST split using overload without prefix completed.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during SplitInto: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
