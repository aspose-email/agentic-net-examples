using Aspose.Email;
using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email.Storage.Pst;

namespace PSTCreationSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Path of the PST file to be created
                string pstFilePath = "output.pst";

                // Ensure the target directory exists
                string directoryPath = Path.GetDirectoryName(pstFilePath);
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Progress reporting delegate
                Action<string> reportProgress = message => Console.WriteLine(message);

                reportProgress("Starting asynchronous PST creation...");

                // Asynchronously create the PST file (Unicode version)
                using (PersonalStorage pst = await PersonalStorage.CreateAsync(pstFilePath, FileFormatVersion.Unicode))
                {
                    reportProgress("PST file created successfully.");
                    // Additional PST operations can be performed here
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
