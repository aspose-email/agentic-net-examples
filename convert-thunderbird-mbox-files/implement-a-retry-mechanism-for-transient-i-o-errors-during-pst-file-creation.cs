using Aspose.Email.Mapi;
using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates retry logic for transient I/O errors while creating a PST file.
            const string pstPath = "output.pst";
            const int maxAttempts = 3;
            int attempt = 0;
            int delayMs = 1000; // initial back‑off delay

            // Ensure the target directory exists
            string directory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            PersonalStorage pst = null;
            try
            {
                while (true)
                {
                    try
                    {
                        // Create the PST file (Unicode version)
                        pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                        break; // success
                    }
                    catch (IOException ioEx)
                    {
                        attempt++;
                        if (attempt >= maxAttempts)
                        {
                            Console.Error.WriteLine($"Failed to create PST after {attempt} attempts: {ioEx.Message}");
                            return;
                        }

                        Console.Error.WriteLine($"Transient I/O error (attempt {attempt}): {ioEx.Message}");
                        Thread.Sleep(delayMs);
                        delayMs *= 2; // exponential back‑off
                    }
                }

                // Example: add a subfolder to the PST root
                FolderInfo rootFolder = pst.RootFolder;
                rootFolder.AddSubFolder("MyFolder");

                // Example: add a simple message to the new folder
                MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test", "This is a test message.");
                FolderInfo subFolder = rootFolder.GetSubFolder("MyFolder");
                subFolder.AddMessage(MapiMessage.FromMailMessage(message));
            }
            finally
            {
                pst?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
