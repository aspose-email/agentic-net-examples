using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            if (!File.Exists(pstPath))
            {
                // Create a new PST file if it does not exist
                try
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Subscribe to PST events
                        pst.ItemMoved += (sender, e) => Console.WriteLine("Item moved event triggered.");
                        pst.StorageProcessing += (sender, e) => Console.WriteLine("Storage processing event triggered.");
                        pst.StorageProcessed += (sender, e) => Console.WriteLine("Storage processed event triggered.");

                        Console.WriteLine("Created new PST file and subscribed to events.");
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating PST file: {ex.Message}");
                    return;
                }
            }
            else
            {
                // Open existing PST file
                try
                {
                    using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                    {
                        // Subscribe to PST events
                        pst.ItemMoved += (sender, e) => Console.WriteLine("Item moved event triggered.");
                        pst.StorageProcessing += (sender, e) => Console.WriteLine("Storage processing event triggered.");
                        pst.StorageProcessed += (sender, e) => Console.WriteLine("Storage processed event triggered.");

                        Console.WriteLine("Opened existing PST file and subscribed to events.");
                        Console.WriteLine("Press any key to exit.");
                        Console.ReadKey();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error opening PST file: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
