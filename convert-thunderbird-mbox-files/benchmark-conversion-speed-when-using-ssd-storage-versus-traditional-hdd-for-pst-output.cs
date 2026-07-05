using Aspose.Email;
using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Input MBOX file path
            string mboxPath = "input.mbox";

            // Ensure the MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(mboxPath) ?? ".");
                    // Minimal MBOX content with a single empty message
                    File.WriteAllText(mboxPath, "From - Mon Jan 01 00:00:00 2020\r\nSubject: Test\r\n\r\n");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Output PST paths for SSD and HDD simulations
            string ssdPstPath = "ssd_output.pst";
            string hddPstPath = "hdd_output.pst";

            // Ensure output directories exist
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ssdPstPath) ?? ".");
                Directory.CreateDirectory(Path.GetDirectoryName(hddPstPath) ?? ".");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directories: {ex.Message}");
                return;
            }

            // Benchmark conversion to SSD location
            Stopwatch ssdTimer = new Stopwatch();
            try
            {
                ssdTimer.Start();
                PersonalStorage ssdPst = MailStorageConverter.MboxToPst(mboxPath, ssdPstPath);
                ssdPst.Dispose();
                ssdTimer.Stop();
                Console.WriteLine($"SSD conversion time: {ssdTimer.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"SSD conversion failed: {ex.Message}");
                return;
            }

            // Benchmark conversion to HDD location
            Stopwatch hddTimer = new Stopwatch();
            try
            {
                hddTimer.Start();
                PersonalStorage hddPst = MailStorageConverter.MboxToPst(mboxPath, hddPstPath);
                hddPst.Dispose();
                hddTimer.Stop();
                Console.WriteLine($"HDD conversion time: {hddTimer.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"HDD conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
