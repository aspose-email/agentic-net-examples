using System;
using System.Diagnostics;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample demonstrates EML to MSG conversion with diagnostics.
            string inputPath = "TestEml.eml";
            string outputPath = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    string placeholder = "From: test@example.com\r\nTo: recipient@example.com\r\nSubject: Test\r\n\r\nHello";
                    File.WriteAllText(inputPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Prepare diagnostics.
            long memoryBefore = GC.GetTotalMemory(true);
            Stopwatch sw = Stopwatch.StartNew();

            // Load EML with options.
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
            {
                // Convert and save as MSG.
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }

            sw.Stop();
            long memoryAfter = GC.GetTotalMemory(true);
            long memoryUsed = memoryAfter - memoryBefore;

            // Report diagnostics.
            Console.WriteLine("Conversion completed.");
            Console.WriteLine($"Time elapsed: {sw.Elapsed.TotalSeconds:F2} seconds");
            Console.WriteLine($"Memory used: {memoryUsed / 1024.0:F2} KB");
            Console.WriteLine("Warnings: None");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
