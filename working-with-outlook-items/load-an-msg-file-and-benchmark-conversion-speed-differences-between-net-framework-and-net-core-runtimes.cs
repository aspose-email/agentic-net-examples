using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "sample.msg";

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body"))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Detect runtime framework
            string frameworkDescription = RuntimeInformation.FrameworkDescription;
            Console.WriteLine($"Running on: {frameworkDescription}");

            // Benchmark loading MSG into MailMessage
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                using (MailMessage mailMessage = MailMessage.Load(
                    msgFilePath,
                    new MsgLoadOptions()))
                {
                    // No further processing needed for benchmark
                }
                stopwatch.Stop();
                Console.WriteLine($"Load time: {stopwatch.ElapsedMilliseconds} ms");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during MSG load: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
