using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "sample.mbox";

            // Output directories for split results
            string syncOutputDir = "SyncChunks";
            string asyncOutputDir = "AsyncChunks";

            // Ensure input MBOX exists; create minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    string placeholder = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Test\r\n\r\nBody\r\n";
                    File.WriteAllText(mboxPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directories exist
            try
            {
                if (!Directory.Exists(syncOutputDir))
                {
                    Directory.CreateDirectory(syncOutputDir);
                }
                if (!Directory.Exists(asyncOutputDir))
                {
                    Directory.CreateDirectory(asyncOutputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directories: {ex.Message}");
                return;
            }

            // Common load options
            MboxLoadOptions loadOptions = new MboxLoadOptions();

            // Chunk size for splitting (1 MB)
            long chunkSize = 1024 * 1024;

            // ---------- Synchronous split ----------
            try
            {
                using (MboxStorageReader syncReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
                {
                    // Validation: read a message using ReadNextMessage()
                    using (MailMessage firstMessage = syncReader.ReadNextMessage())
                    {
                        // No further processing needed
                    }

                    Stopwatch syncStopwatch = new Stopwatch();
                    syncStopwatch.Start();

                    syncReader.SplitInto(chunkSize, syncOutputDir);

                    syncStopwatch.Stop();
                    Console.WriteLine($"Synchronous split elapsed: {syncStopwatch.Elapsed}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Synchronous split failed: {ex.Message}");
                return;
            }

            // ---------- Asynchronous split ----------
            try
            {
                using (MboxStorageReader asyncReader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
                {
                    // Validation: read a message using ReadNextMessage()
                    using (MailMessage firstMessage = asyncReader.ReadNextMessage())
                    {
                        // No further processing needed
                    }

                    Stopwatch asyncStopwatch = new Stopwatch();
                    asyncStopwatch.Start();

                    CancellationTokenSource cts = new CancellationTokenSource();
                    Task splitTask = asyncReader.SplitIntoAsync(chunkSize, asyncOutputDir, cts.Token);
                    await splitTask.ConfigureAwait(false);

                    asyncStopwatch.Stop();
                    Console.WriteLine($"Asynchronous split elapsed: {asyncStopwatch.Elapsed}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Asynchronous split failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
