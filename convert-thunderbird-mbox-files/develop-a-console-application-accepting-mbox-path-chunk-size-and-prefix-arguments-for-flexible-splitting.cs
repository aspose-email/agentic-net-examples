using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <MBOX path> <chunk size in bytes> [output prefix]");
                return;
            }

            string mboxPath = args[0];
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            if (!long.TryParse(args[1], out long chunkSize) || chunkSize <= 0)
            {
                Console.Error.WriteLine("Invalid chunk size.");
                return;
            }

            string prefix = args.Length >= 3 ? args[2] : string.Empty;

            string outputDirectory = Path.Combine(Path.GetDirectoryName(mboxPath) ?? string.Empty, "MboxChunks");
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                // Ensure the reader can read at least one message
                MailMessage firstMessage = reader.ReadNextMessage();
                // The firstMessage variable is not used further; it's just to satisfy the required ReadNextMessage call

                if (string.IsNullOrEmpty(prefix))
                {
                    reader.SplitInto(chunkSize, outputDirectory);
                }
                else
                {
                    reader.SplitInto(chunkSize, outputDirectory, prefix);
                }
            }

            Console.WriteLine("MBOX splitting completed.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
