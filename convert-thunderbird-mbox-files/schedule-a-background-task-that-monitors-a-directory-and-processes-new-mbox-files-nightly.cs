using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    // Directory to watch for new MBOX files
    private const string WatchDirectory = "MboxInbox";
    // Directory where extracted messages will be saved
    private const string OutputDirectory = "ExtractedMessages";

    static void Main(string[] args)
    {
        try
        {
            // Ensure required directories exist
            if (!Directory.Exists(WatchDirectory))
            {
                Console.Error.WriteLine($"Watch directory '{WatchDirectory}' does not exist. Creating it.");
                Directory.CreateDirectory(WatchDirectory);
            }

            if (!Directory.Exists(OutputDirectory))
            {
                Directory.CreateDirectory(OutputDirectory);
            }

            // Schedule the nightly processing task (runs once every 24 hours)
            Timer timer = new Timer(
                callback: state => ProcessPendingMboxFiles(),
                state: null,
                dueTime: GetInitialDelay(),
                period: TimeSpan.FromHours(24).Milliseconds);

            // Keep the application running
            Console.WriteLine("MBOX monitor started. Press Enter to exit.");
            Console.ReadLine();

            // Dispose timer before exit
            timer.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Calculates the delay until the next midnight (or a chosen nightly time)
    private static int GetInitialDelay()
    {
        DateTime now = DateTime.Now;
        DateTime nextRun = now.Date.AddDays(1).AddHours(2); // 02:00 AM next day
        TimeSpan delay = nextRun - now;
        return (int)delay.TotalMilliseconds;
    }

    // Scans the watch directory for *.mbox files and processes each one
    private static void ProcessPendingMboxFiles()
    {
        try
        {
            string[] mboxFiles = Directory.GetFiles(WatchDirectory, "*.mbox");
            foreach (string mboxFilePath in mboxFiles)
            {
                ProcessMboxFile(mboxFilePath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error while scanning directory: {ex.Message}");
        }
    }

    // Reads an MBOX file sequentially and saves each message as an .eml file
    private static void ProcessMboxFile(string mboxFilePath)
    {
        if (!File.Exists(mboxFilePath))
        {
            Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
            return;
        }

        try
        {
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
            {
                MailMessage message;
                while ((message = reader.ReadNextMessage()) != null)
                {
                    try
                    {
                        string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                        // Replace invalid filename characters
                        foreach (char invalidChar in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(invalidChar, '_');
                        }

                        string emlFileName = Path.Combine(OutputDirectory, $"{safeSubject}_{Guid.NewGuid()}.eml");
                        using (MailMessage disposableMessage = message)
                        {
                            disposableMessage.Save(emlFileName);
                        }

                        Console.WriteLine($"Extracted message to: {emlFileName}");
                    }
                    catch (Exception msgEx)
                    {
                        Console.Error.WriteLine($"Failed to save a message from '{mboxFilePath}': {msgEx.Message}");
                    }
                }
            }

            // Optionally move processed MBOX file to an archive folder
            string archiveDir = Path.Combine(WatchDirectory, "Processed");
            if (!Directory.Exists(archiveDir))
            {
                Directory.CreateDirectory(archiveDir);
            }
            string destPath = Path.Combine(archiveDir, Path.GetFileName(mboxFilePath));
            File.Move(mboxFilePath, destPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MBOX file '{mboxFilePath}': {ex.Message}");
        }
    }
}
