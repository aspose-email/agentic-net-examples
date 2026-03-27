using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "storage.pst";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage createdPst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional setup required for an empty PST
                    }
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file for reading/writing
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Set up a file system watcher to monitor changes to the PST file
                    string directory = Path.GetDirectoryName(pstPath);
                    string fileName = Path.GetFileName(pstPath);

                    using (FileSystemWatcher watcher = new FileSystemWatcher())
                    {
                        watcher.Path = directory ?? ".";
                        watcher.Filter = fileName;
                        watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

                        watcher.Changed += (object sender, FileSystemEventArgs e) =>
                        {
                            Console.WriteLine($"PST file changed: {e.FullPath}");
                        };

                        watcher.EnableRaisingEvents = true;

                        Console.WriteLine("Monitoring PST file for changes. Press Enter to exit.");
                        Console.ReadLine();
                    }
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error accessing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
