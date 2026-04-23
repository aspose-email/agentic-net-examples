using Aspose.Email;
using System;
using System.IO;
using System.Text;
using System.Threading;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if it does not.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholderStream = new FileStream(mboxPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    {
                        // Minimal MBOX content: a single empty message.
                        string placeholderContent = "From - Mon Jan 01 00:00:00 2000\r\nSubject: Placeholder\r\n\r\n";
                        byte[] bytes = Encoding.UTF8.GetBytes(placeholderContent);
                        placeholderStream.Write(bytes, 0, bytes.Length);
                    }
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Retry logic for opening the MBOX file stream.
            const int maxRetries = 3;
            const int delayMilliseconds = 1000;
            FileStream mboxStream = null;
            int attempt = 0;
            while (attempt < maxRetries)
            {
                try
                {
                    mboxStream = new FileStream(mboxPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                    break; // Successfully opened.
                }
                catch (IOException ioEx)
                {
                    attempt++;
                    Console.Error.WriteLine($"Attempt {attempt} to open MBOX file failed: {ioEx.Message}");
                    if (attempt >= maxRetries)
                    {
                        Console.Error.WriteLine("Maximum retry attempts reached. Aborting.");
                        return;
                    }
                    Thread.Sleep(delayMilliseconds);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error opening MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert the MBOX stream to PST.
            try
            {
                using (FileStream mboxStreamDisposable = mboxStream)
                {
                    using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxStreamDisposable, pstPath))
                    {
                        Console.WriteLine($"MBOX converted to PST successfully. PST saved at '{pstPath}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
