using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "large.mbox";
            const string pstDirectPath = "output_direct.pst";
            const string pstBatchPath = "output_batch.pst";

            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (var writer = new StreamWriter(mboxPath))
                    {
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Sample Message");
                        writer.WriteLine("From: sender@example.com");
                        writer.WriteLine("To: recipient@example.com");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder message.");
                        writer.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Direct conversion (no batching)
            long memBeforeDirect = GC.GetTotalMemory(true);
            Stopwatch swDirect = Stopwatch.StartNew();

            try
            {
                using (PersonalStorage pstDirect = MailStorageConverter.MboxToPst(mboxPath, pstDirectPath))
                {
                    // No additional work needed
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Direct conversion failed: {ex.Message}");
                return;
            }

            swDirect.Stop();
            long memAfterDirect = GC.GetTotalMemory(true);
            Console.WriteLine("Direct conversion:");
            Console.WriteLine($"  Time elapsed: {swDirect.Elapsed}");
            Console.WriteLine($"  Memory used: {(memAfterDirect - memBeforeDirect) / 1024} KB");

            // Batch conversion using MboxStorageReader
            long memBeforeBatch = GC.GetTotalMemory(true);
            Stopwatch swBatch = Stopwatch.StartNew();

            try
            {
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                using (PersonalStorage pstBatch = PersonalStorage.Create(pstBatchPath, FileFormatVersion.Unicode))
                {
                    // Create a folder in PST to hold imported messages.
                    FolderInfo pstFolder = pstBatch.RootFolder.AddSubFolder("Imported");

                    const int batchSize = 1000;
                    List<MboxMessageInfo> batch = new List<MboxMessageInfo>(batchSize);

                    foreach (MboxMessageInfo info in mboxReader.EnumerateMessageInfo())
                    {
                        batch.Add(info);
                        if (batch.Count >= batchSize)
                        {
                            ProcessBatch(batch, mboxReader, pstFolder);
                            batch.Clear();
                        }
                    }

                    if (batch.Count > 0)
                    {
                        ProcessBatch(batch, mboxReader, pstFolder);
                        batch.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Batch conversion failed: {ex.Message}");
                return;
            }

            swBatch.Stop();
            long memAfterBatch = GC.GetTotalMemory(true);
            Console.WriteLine("Batch conversion:");
            Console.WriteLine($"  Time elapsed: {swBatch.Elapsed}");
            Console.WriteLine($"  Memory used: {(memAfterBatch - memBeforeBatch) / 1024} KB");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessBatch(List<MboxMessageInfo> batch, MboxStorageReader reader, FolderInfo pstFolder)
    {
        foreach (MboxMessageInfo info in batch)
        {
            MailMessage message = reader.ExtractMessage(info.EntryId, new EmlLoadOptions());
            pstFolder.AddMessage(MapiMessage.FromMailMessage(message));
            message.Dispose();
        }
    }
}
