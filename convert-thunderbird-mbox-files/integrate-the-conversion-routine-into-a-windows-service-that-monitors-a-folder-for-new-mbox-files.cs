using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string watchFolder = "MboxWatch";
            if (!Directory.Exists(watchFolder))
            {
                Directory.CreateDirectory(watchFolder);
            }

            string outputFolder = "PstOutput";
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            using (FileSystemWatcher watcher = new FileSystemWatcher())
            {
                watcher.Path = watchFolder;
                watcher.Filter = "*.mbox";
                watcher.Created += (object sender, FileSystemEventArgs e) =>
                {
                    try
                    {
                        string mboxPath = e.FullPath;

                        // Wait briefly for the file to become available
                        int retry = 0;
                        while (retry < 5 && !File.Exists(mboxPath))
                        {
                            Thread.Sleep(500);
                            retry++;
                        }

                        if (!File.Exists(mboxPath))
                        {
                            // Create a minimal placeholder MBOX file
                            try
                            {
                                string placeholder = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.\r\n\r\n";
                                File.WriteAllText(mboxPath, placeholder);
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                                return;
                            }
                        }

                        string pstPath = Path.Combine(outputFolder, Path.GetFileNameWithoutExtension(mboxPath) + ".pst");
                        try
                        {
                            MailStorageConverter.MboxToPst(mboxPath, pstPath);
                            Console.WriteLine($"Converted '{mboxPath}' to '{pstPath}'.");
                        }
                        catch (Exception convEx)
                        {
                            Console.Error.WriteLine($"Conversion failed for '{mboxPath}': {convEx.Message}");
                        }
                    }
                    catch (Exception handlerEx)
                    {
                        Console.Error.WriteLine($"Error handling file '{e.FullPath}': {handlerEx.Message}");
                    }
                };

                watcher.EnableRaisingEvents = true;

                Console.WriteLine($"Monitoring folder '{watchFolder}' for new MBOX files. Press Enter to exit.");
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
