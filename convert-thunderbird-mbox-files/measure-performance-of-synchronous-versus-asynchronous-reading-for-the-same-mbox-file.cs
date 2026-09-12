using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static async Task Main(string[] args)
    {
        // Placeholder path to the MBOX file – replace with a valid path when testing
        string mboxPath = "path/to/your.mbox";

        if (!File.Exists(mboxPath))
        {
            Console.WriteLine($"MBOX file not found: {mboxPath}");
            return;
        }

        // Synchronous reading
        int syncCount = 0;
        var syncStopwatch = Stopwatch.StartNew();

        using (var syncReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
        {
            MailMessage message;
            while ((message = syncReader.ReadNextMessage()) != null)
            {
                syncCount++;
                // Process the message if needed
            }
        }

        syncStopwatch.Stop();
        Console.WriteLine($"Synchronous read: {syncCount} messages in {syncStopwatch.ElapsedMilliseconds} ms");

        // Asynchronous reading (simulated with Task.Run)
        int asyncCount = 0;
        var asyncStopwatch = Stopwatch.StartNew();

        using (var asyncReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
        {
            while (true)
            {
                MailMessage asyncMessage = await Task.Run(() => asyncReader.ReadNextMessage(), CancellationToken.None);
                if (asyncMessage == null)
                    break;

                asyncCount++;
                // Process the message if needed
            }
        }

        asyncStopwatch.Stop();
        Console.WriteLine($"Asynchronous read: {asyncCount} messages in {asyncStopwatch.ElapsedMilliseconds} ms");
    }
}
