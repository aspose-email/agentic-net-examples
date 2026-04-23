using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists.
            try
            {
                if (!File.Exists(mboxPath))
                {
                    // Create a minimal placeholder MBOX file.
                    const string placeholder = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\nThis is a placeholder message.\r\n";
                    File.WriteAllText(mboxPath, placeholder);
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
                return;
            }

            // Synchronous reading.
            try
            {
                using (MboxStorageReader syncReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    Stopwatch syncWatch = Stopwatch.StartNew();
                    int syncCount = 0;
                    MailMessage syncMessage;
                    while ((syncMessage = syncReader.ReadNextMessage()) != null)
                    {
                        syncCount++;
                        syncMessage.Dispose();
                    }
                    syncWatch.Stop();
                    Console.WriteLine($"Synchronous read: {syncWatch.ElapsedMilliseconds} ms, messages read: {syncCount}");
                }
            }
            catch (Exception syncEx)
            {
                Console.Error.WriteLine($"Synchronous read error: {syncEx.Message}");
                return;
            }

            // Asynchronous creation + synchronous reading.
            try
            {
                MboxStorageReader asyncReader = null;
                try
                {
                    asyncReader = MboxStorageReader.CreateReaderAsync(mboxPath, new MboxLoadOptions()).GetAwaiter().GetResult();
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Async reader creation error: {createEx.Message}");
                    return;
                }

                using (asyncReader)
                {
                    Stopwatch asyncWatch = Stopwatch.StartNew();
                    int asyncCount = 0;
                    MailMessage asyncMessage;
                    while ((asyncMessage = asyncReader.ReadNextMessage()) != null)
                    {
                        asyncCount++;
                        asyncMessage.Dispose();
                    }
                    asyncWatch.Stop();
                    Console.WriteLine($"Async creation read: {asyncWatch.ElapsedMilliseconds} ms, messages read: {asyncCount}");
                }
            }
            catch (Exception asyncEx)
            {
                Console.Error.WriteLine($"Asynchronous read error: {asyncEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
