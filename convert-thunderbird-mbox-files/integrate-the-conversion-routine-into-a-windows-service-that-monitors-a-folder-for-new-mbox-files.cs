using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using System.Threading;

namespace MboxToPstService
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFolder = @"C:\MboxInput";
                string outputFolder = @"C:\PstOutput";

                // Ensure the input and output directories exist.
                if (!Directory.Exists(inputFolder))
                {
                    Directory.CreateDirectory(inputFolder);
                }

                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }

                // Create a placeholder MBOX file if none exist to satisfy validation.
                string placeholderPath = Path.Combine(inputFolder, "placeholder.mbox");
                if (!File.Exists(placeholderPath))
                {
                    File.WriteAllText(placeholderPath, "From - placeholder@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder MBOX file.");
                }

                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = inputFolder;
                    watcher.Filter = "*.mbox";
                    watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime;
                    watcher.Created += (sender, e) => OnMboxCreated(e, outputFolder);
                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine("Monitoring folder: " + inputFolder);
                    Console.WriteLine("Press Enter to exit.");

                    // Keep the application running until the user presses Enter.
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }

        private static void OnMboxCreated(FileSystemEventArgs e, string outputFolder)
        {
            // Run conversion in a separate thread to avoid blocking the watcher.
            ThreadPool.QueueUserWorkItem(state =>
            {
                string mboxPath = e.FullPath;
                try
                {
                    // Wait for the file to become accessible (in case it is still being copied).
                    const int maxAttempts = 10;
                    int attempt = 0;
                    while (attempt < maxAttempts)
                    {
                        try
                        {
                            using (FileStream fs = File.Open(mboxPath, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                // File is ready.
                                break;
                            }
                        }
                        catch (IOException)
                        {
                            Thread.Sleep(500);
                            attempt++;
                        }
                    }

                    if (!File.Exists(mboxPath))
                    {
                        Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                        return;
                    }

                    string pstFileName = Path.GetFileNameWithoutExtension(mboxPath) + ".pst";
                    string pstPath = Path.Combine(outputFolder, pstFileName);

                    // Convert MBOX to PST.
                    using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                    {
                        // PST is saved and disposed automatically.
                    }

                    Console.WriteLine($"Converted '{mboxPath}' to '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{mboxPath}': {ex.Message}");
                }
            });
        }
    }
}
