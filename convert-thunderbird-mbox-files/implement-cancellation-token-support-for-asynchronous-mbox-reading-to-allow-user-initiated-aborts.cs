using System;
using System.IO;
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
            string mboxPath = "sample.mbox";
            string outputDir = "output";

            // Guard file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (CancellationTokenSource cts = new CancellationTokenSource())
            {
                // Allow user to cancel with Ctrl+C
                Console.CancelKeyPress += (s, e) =>
                {
                    e.Cancel = true;
                    cts.Cancel();
                    Console.WriteLine("Cancellation requested...");
                };

                // Asynchronously create the reader
                MboxStorageReader reader = await MboxStorageReader.CreateReaderAsync(
                    mboxPath,
                    new MboxLoadOptions(),
                    cts.Token);

                using (reader)
                {
                    int messageIndex = 0;
                    while (!cts.Token.IsCancellationRequested)
                    {
                        // Read next message sequentially
                        MailMessage message = reader.ReadNextMessage();
                        if (message == null)
                            break; // No more messages

                        using (message)
                        {
                            string safeSubject = string.IsNullOrEmpty(message.Subject) ? "NoSubject" : message.Subject;
                            string fileName = Path.Combine(outputDir, $"Message_{++messageIndex}_{safeSubject}.eml");

                            try
                            {
                                message.Save(fileName);
                                Console.WriteLine($"Saved: {fileName}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
