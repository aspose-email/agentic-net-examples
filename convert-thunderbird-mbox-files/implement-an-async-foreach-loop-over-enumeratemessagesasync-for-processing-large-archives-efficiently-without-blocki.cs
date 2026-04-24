using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            string pstPath = "archive.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Process messages asynchronously using async foreach.
            await foreach (MapiMessage message in EnumerateMessagesAsync(pstPath))
            {
                Console.WriteLine($"Subject: {message.Subject}");
                // Additional processing can be done here.
                message.Dispose(); // Dispose each message after use.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Asynchronously enumerates messages from a PST file.
    private static async IAsyncEnumerable<MapiMessage> EnumerateMessagesAsync(string pstFilePath, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        // Load the PST file asynchronously.
        PersonalStorage pst;
        try
        {
            pst = await PersonalStorage.FromFileAsync(pstFilePath, cancellationToken);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load PST file: {ex.Message}");
            yield break;
        }

        using (pst)
        {
            // Access the Inbox folder (or any other folder as needed).
            FolderInfo inboxFolder;
            try
            {
                inboxFolder = pst.RootFolder.GetSubFolder("Inbox");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Inbox folder not found: {ex.Message}");
                yield break;
            }

            // Enumerate messages synchronously but yield them asynchronously.
            foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
            {
                cancellationToken.ThrowIfCancellationRequested();

                MapiMessage mapiMessage;
                try
                {
                    mapiMessage = pst.ExtractMessage(messageInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to extract message: {ex.Message}");
                    continue;
                }

                yield return mapiMessage;
            }
        }
    }
}
