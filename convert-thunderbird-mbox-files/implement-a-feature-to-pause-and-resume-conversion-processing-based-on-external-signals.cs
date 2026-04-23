using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

namespace ConvertMboxWithPauseResume
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input and output paths
                string mboxPath = "input.mbox";
                string pstPath = "output.pst";

                // Verify input file exists
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                // Ensure output directory exists
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Pause/Resume control
                ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);

                // Background thread to listen for console commands
                Thread commandThread = new Thread(() =>
                {
                    Console.WriteLine("Press 'p' to pause, 'r' to resume, 'q' to quit.");
                    while (true)
                    {
                        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                        if (keyInfo.KeyChar == 'p')
                        {
                            pauseEvent.Reset();
                            Console.WriteLine("Processing paused.");
                        }
                        else if (keyInfo.KeyChar == 'r')
                        {
                            pauseEvent.Set();
                            Console.WriteLine("Processing resumed.");
                        }
                        else if (keyInfo.KeyChar == 'q')
                        {
                            Console.WriteLine("Exiting.");
                            Environment.Exit(0);
                        }
                    }
                });
                commandThread.IsBackground = true;
                commandThread.Start();

                // Set up conversion options with a message handler that respects pause/resume
                MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
                conversionOptions.MessageHandler = new MailStorageConverter.MailHandler((MailMessage message) =>
                {
                    // Wait here if the process is paused
                    pauseEvent.Wait();

                    // Example: you could modify the message here if needed
                    // For this sample we leave it unchanged
                });

                // Perform the conversion inside a using block to ensure disposal
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, conversionOptions))
                {
                    // Conversion completed
                    Console.WriteLine("MBOX to PST conversion completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
