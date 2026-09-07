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
                string? pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Pause/Resume control
                ManualResetEventSlim pauseEvent = new ManualResetEventSlim(true);
                string pauseSignalPath = "pause.signal";
                string resumeSignalPath = "resume.signal";
                string stopSignalPath = "stop.signal";

                // Set up conversion options with a message handler that respects pause/resume
                MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
                conversionOptions.MessageHandler = new MailStorageConverter.MailHandler((MailMessage message) =>
                {
                    if (File.Exists(stopSignalPath))
                    {
                        throw new OperationCanceledException("Conversion stopped by external signal.");
                    }

                    if (File.Exists(pauseSignalPath))
                    {
                        pauseEvent.Reset();
                    }

                    while (!pauseEvent.IsSet)
                    {
                        if (File.Exists(stopSignalPath))
                        {
                            throw new OperationCanceledException("Conversion stopped by external signal.");
                        }

                        if (!File.Exists(pauseSignalPath) || File.Exists(resumeSignalPath))
                        {
                            pauseEvent.Set();
                            if (File.Exists(resumeSignalPath))
                            {
                                File.Delete(resumeSignalPath);
                            }
                            break;
                        }

                        Thread.Sleep(250);
                    }

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
