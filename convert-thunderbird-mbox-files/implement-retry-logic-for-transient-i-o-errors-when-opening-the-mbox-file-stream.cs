using System;
using System.IO;
using System.Threading;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";

            // Ensure the MBOX file exists; create an empty placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Retry logic for transient I/O errors when opening the MBOX reader.
            const int maxRetries = 3;
            int attempt = 0;
            MboxStorageReader mbox = null;

            while (true)
            {
                try
                {
                    mbox = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());
                    break; // Success
                }
                catch (IOException ioEx)
                {
                    attempt++;
                    if (attempt >= maxRetries)
                    {
                        Console.Error.WriteLine($"Unable to open MBOX file after {maxRetries} attempts: {ioEx.Message}");
                        return;
                    }
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error while opening MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists.
            string outputDir = "output";
            Directory.CreateDirectory(outputDir);

            using (mbox)
            {
                while (true)
                {
                    MailMessage eml = mbox.ReadNextMessage();
                    if (eml == null)
                        break;

                    using (eml)
                    {
                        string safeSubject = string.IsNullOrWhiteSpace(eml.Subject) ? "NoSubject" : eml.Subject;
                        foreach (char c in Path.GetInvalidFileNameChars())
                        {
                            safeSubject = safeSubject.Replace(c, '_');
                        }

                        string outputPath = Path.Combine(outputDir, $"{safeSubject}.eml");
                        try
                        {
                            eml.Save(outputPath);
                            Console.WriteLine($"Saved: {outputPath}");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save message '{safeSubject}': {saveEx.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
