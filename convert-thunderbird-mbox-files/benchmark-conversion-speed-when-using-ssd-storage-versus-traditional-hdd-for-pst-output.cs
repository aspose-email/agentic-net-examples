using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for the source MBOX and the PST outputs (SSD and HDD)
            string mboxPath = "input.mbox";
            string ssdPstPath = "ssd_output.pst";
            string hddPstPath = "hdd_output.pst";

            // Ensure the source MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    string placeholderEmail = "From - Mon Jan 01 00:00:00 2020\r\nSubject: Placeholder\r\n\r\nThis is a placeholder email.\r\n";
                    File.WriteAllText(mboxPath, placeholderEmail);
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                    return;
                }
            }

            // Ensure output directories exist
            try
            {
                string ssdDir = Path.GetDirectoryName(ssdPstPath);
                if (!string.IsNullOrEmpty(ssdDir) && !Directory.Exists(ssdDir))
                {
                    Directory.CreateDirectory(ssdDir);
                }

                string hddDir = Path.GetDirectoryName(hddPstPath);
                if (!string.IsNullOrEmpty(hddDir) && !Directory.Exists(hddDir))
                {
                    Directory.CreateDirectory(hddDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directories: {dirEx.Message}");
                return;
            }

            // Benchmark conversion to SSD PST
            Stopwatch ssdTimer = new Stopwatch();
            ssdTimer.Start();
            try
            {
                using (PersonalStorage ssdPst = MailStorageConverter.MboxToPst(mboxPath, ssdPstPath))
                {
                    // PST is created and disposed automatically
                }
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"SSD conversion failed: {convEx.Message}");
                return;
            }
            ssdTimer.Stop();
            TimeSpan ssdDuration = ssdTimer.Elapsed;

            // Benchmark conversion to HDD PST
            Stopwatch hddTimer = new Stopwatch();
            hddTimer.Start();
            try
            {
                using (PersonalStorage hddPst = MailStorageConverter.MboxToPst(mboxPath, hddPstPath))
                {
                    // PST is created and disposed automatically
                }
            }
            catch (Exception convEx)
            {
                Console.Error.WriteLine($"HDD conversion failed: {convEx.Message}");
                return;
            }
            hddTimer.Stop();
            TimeSpan hddDuration = hddTimer.Elapsed;

            // Output benchmark results
            Console.WriteLine($"SSD conversion time: {ssdDuration.TotalMilliseconds} ms");
            Console.WriteLine($"HDD conversion time: {hddDuration.TotalMilliseconds} ms");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
