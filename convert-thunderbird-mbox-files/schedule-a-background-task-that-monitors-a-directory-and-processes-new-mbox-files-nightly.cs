using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string inputDirectory = "MboxInput";
            string outputDirectory = "EmlOutput";
            string archiveDirectory = "MboxArchive";

            Directory.CreateDirectory(inputDirectory);
            Directory.CreateDirectory(outputDirectory);
            Directory.CreateDirectory(archiveDirectory);

            var cts = new CancellationTokenSource();

            Task.Run(async () =>
            {
                while (!cts.Token.IsCancellationRequested)
                {
                    try
                    {
                        ProcessPendingMboxFiles(inputDirectory, outputDirectory, archiveDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during MBOX processing: {ex.Message}");
                    }

                    await Task.Delay(TimeSpan.FromHours(24), cts.Token).ConfigureAwait(false);
                }
            }, cts.Token);

            Console.WriteLine("MBOX monitor started. Press any key to exit...");
            Console.ReadKey();
            cts.Cancel();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }

    private static void ProcessPendingMboxFiles(string inputDir, string outputDir, string archiveDir)
    {
        string[] mboxFiles = Directory.GetFiles(inputDir, "*.mbox", SearchOption.TopDirectoryOnly);
        foreach (string mboxPath in mboxFiles)
        {
            try
            {
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    continue;
                }

                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;
                    while (true)
                    {
                        MailMessage emlMessage = mboxReader.ReadNextMessage();
                        if (emlMessage == null)
                            break;

                        try
                        {
                            string safeSubject = string.IsNullOrWhiteSpace(emlMessage.Subject) ? "NoSubject" : emlMessage.Subject;
                            foreach (char c in Path.GetInvalidFileNameChars())
                                safeSubject = safeSubject.Replace(c, '_');

                            string emlFileName = $"{safeSubject}_{messageIndex}.eml";
                            string emlPath = Path.Combine(outputDir, emlFileName);

                            emlMessage.Save(emlPath);
                        }
                        catch (Exception exMsg)
                        {
                            Console.Error.WriteLine($"Failed to process message #{messageIndex} in {Path.GetFileName(mboxPath)}: {exMsg.Message}");
                        }

                        messageIndex++;
                    }
                }

                string archivePath = Path.Combine(archiveDir, Path.GetFileName(mboxPath));
                if (File.Exists(archivePath))
                    File.Delete(archivePath);
                File.Move(mboxPath, archivePath);
            }
            catch (Exception exFile)
            {
                Console.Error.WriteLine($"Failed to process MBOX file {mboxPath}: {exFile.Message}");
            }
        }
    }
}
