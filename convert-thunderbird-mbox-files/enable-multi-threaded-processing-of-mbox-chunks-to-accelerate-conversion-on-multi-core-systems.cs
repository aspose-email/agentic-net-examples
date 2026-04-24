using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX path and ensure it exists.
            string mboxPath = "input.mbox";
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream fs = File.Create(mboxPath))
                    {
                        string placeholder = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Test\r\n\r\nThis is a test message.\r\n";
                        byte[] bytes = Encoding.UTF8.GetBytes(placeholder);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Prepare output directory for split chunks.
            string outputDir = "output";
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Split the MBOX into smaller chunks.
            string splitFolder = Path.Combine(outputDir, "chunks");
            try
            {
                if (!Directory.Exists(splitFolder))
                {
                    Directory.CreateDirectory(splitFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create split folder: {ex.Message}");
                return;
            }

            try
            {
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    long chunkSize = 1024 * 1024; // 1 MB per chunk
                    mboxReader.SplitInto(chunkSize, splitFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to split MBOX file: {ex.Message}");
                return;
            }

            // Get all generated chunk files.
            string[] chunkFiles;
            try
            {
                chunkFiles = Directory.GetFiles(splitFolder, "*.mbox");
                if (chunkFiles.Length == 0)
                {
                    Console.Error.WriteLine("No chunk files were created.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate chunk files: {ex.Message}");
                return;
            }

            // Convert each chunk to a separate PST file in parallel.
            Parallel.ForEach(chunkFiles, chunkPath =>
            {
                string pstPath = Path.ChangeExtension(chunkPath, ".pst");
                try
                {
                    MailStorageConverter.MboxToPst(chunkPath, pstPath);
                    Console.WriteLine($"Converted '{Path.GetFileName(chunkPath)}' to '{Path.GetFileName(pstPath)}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert '{chunkPath}': {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
