using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage.Olm;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the OLM file (replace with an actual file path)
            string olmFilePath = "sample.olm";

            // Guard against missing file
            if (!File.Exists(olmFilePath))
            {
                Console.Error.WriteLine($"File not found: {olmFilePath}");
                return;
            }

            // Measure loading time using the constructor
            Stopwatch constructorTimer = Stopwatch.StartNew();
            using (OlmStorage olmConstructor = new OlmStorage(olmFilePath))
            {
                constructorTimer.Stop();
                Console.WriteLine($"Constructor load time: {constructorTimer.ElapsedMilliseconds} ms");

                // Example operation: get total items count
                int totalItemsCtor = olmConstructor.GetTotalItemsCount();
                Console.WriteLine($"Total items (constructor): {totalItemsCtor}");
            }

            // Measure loading time using the static FromFile method
            Stopwatch staticTimer = Stopwatch.StartNew();
            using (OlmStorage olmStatic = OlmStorage.FromFile(olmFilePath))
            {
                staticTimer.Stop();
                Console.WriteLine($"FromFile load time: {staticTimer.ElapsedMilliseconds} ms");

                // Example operation: get total items count
                int totalItemsStatic = olmStatic.GetTotalItemsCount();
                Console.WriteLine($"Total items (FromFile): {totalItemsStatic}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
