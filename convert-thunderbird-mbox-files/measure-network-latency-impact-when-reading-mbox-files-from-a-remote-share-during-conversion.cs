using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string remoteMboxPath = @"\\remote\share\mailbox.mbox";
            string outputPstPath = "output.pst";

            // Ensure the remote MBOX file exists; create a minimal placeholder if missing.
            if (!File.Exists(remoteMboxPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(remoteMboxPath))
                    {
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Test");
                        writer.WriteLine();
                        writer.WriteLine("This is a test message.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST output exists.
            string pstDirectory = Path.GetDirectoryName(outputPstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST output directory: {ex.Message}");
                    return;
                }
            }

            // Measure conversion time (includes reading the remote MBOX file).
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                MailStorageConverter.MboxToPst(remoteMboxPath, outputPstPath);
                stopwatch.Stop();
                Console.WriteLine($"MBOX to PST conversion completed in {stopwatch.ElapsedMilliseconds} ms.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
