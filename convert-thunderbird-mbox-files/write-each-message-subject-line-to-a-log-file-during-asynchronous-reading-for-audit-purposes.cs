using System;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    // Author: Generated example for asynchronous audit logging while reading MBOX messages.
    static async Task Main(string[] args)
    {
        try
        {
            const string mboxPath = "storage.mbox";
            const string logPath = "audit.log";

            // Verify MBOX file exists.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the directory for the log file exists.
            string? logDir = Path.GetDirectoryName(logPath);
            if (!string.IsNullOrEmpty(logDir) && !Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }

            // Open the log file for asynchronous writing.
            await using (StreamWriter logWriter = new StreamWriter(new FileStream(logPath, FileMode.Append, FileAccess.Write, FileShare.Read)))
            {
                // Create the MBOX reader.
                MboxStorageReader mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());

                // Iterate through each message info object.
                foreach (MboxMessageInfo mboxMessageInfo in mbox.EnumerateMessageInfo())
                {
                    // Write subject to console.
                    Console.WriteLine($"Subject: {mboxMessageInfo.Subject}");

                    // Asynchronously log the subject for audit.
                    await logWriter.WriteLineAsync(mboxMessageInfo.Subject);

                    // Extract the full MIME message.
                    MailMessage eml = mbox.ExtractMessage(mboxMessageInfo.EntryId, new EmlLoadOptions());

                    // Save the extracted message as an .eml file (optional).
                    string emlFileName = $"{eml.Subject}.eml";
                    eml.Save(emlFileName);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
