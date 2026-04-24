using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "sender@example.com";
                        placeholder.To = "recipient@example.com";
                        placeholder.Subject = "Placeholder";
                        placeholder.Body = "This is a placeholder message.";
                        placeholder.Save(inputMsgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            List<string> warnings = new List<string>();
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                // Load the MSG as a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(inputMsgPath))
                {
                    // Convert to MailMessage with default options
                    MailConversionOptions conversionOptions = new MailConversionOptions();
                    using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                    {
                        // Save as EML
                        try
                        {
                            mailMessage.Save(outputEmlPath);
                        }
                        catch (Exception ex)
                        {
                            warnings.Add($"Failed to save EML file: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                warnings.Add($"Conversion error: {ex.Message}");
            }

            stopwatch.Stop();

            // Diagnostic report
            Process currentProcess = Process.GetCurrentProcess();
            long peakMemoryBytes = currentProcess.PeakWorkingSet64;
            double durationSeconds = stopwatch.Elapsed.TotalSeconds;

            Console.WriteLine("=== Diagnostic Report ===");
            Console.WriteLine($"Peak Memory Usage: {peakMemoryBytes / 1024 / 1024} MB");
            Console.WriteLine($"Conversion Duration: {durationSeconds:F3} seconds");
            if (warnings.Count > 0)
            {
                Console.WriteLine("Warnings:");
                foreach (string warning in warnings)
                {
                    Console.WriteLine($"- {warning}");
                }
            }
            else
            {
                Console.WriteLine("No warnings.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
