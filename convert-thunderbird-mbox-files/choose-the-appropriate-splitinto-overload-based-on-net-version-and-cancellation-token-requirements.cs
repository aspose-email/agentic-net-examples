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
            string pstPath = "input.pst";
            string outputFolder = "output";
            long chunkSize = 10 * 1024 * 1024; // 10 MB
            string prefix = "part";

            // Ensure the input PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage placeholderPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // No additional content needed for the placeholder.
                }
                Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
            }

            // Ensure the output directory exists.
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Load the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Use the overload that accepts a CancellationToken (available in .NET 5+).
                CancellationTokenSource cancellationSource = new CancellationTokenSource();
                pst.SplitIntoAsync(chunkSize, prefix, outputFolder, cancellationSource.Token).GetAwaiter().GetResult();

                // If a token is not required, the simpler overload can be used:
                // pst.SplitIntoAsync(chunkSize, outputFolder).GetAwaiter().GetResult();
            }

            Console.WriteLine("PST split operation completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
