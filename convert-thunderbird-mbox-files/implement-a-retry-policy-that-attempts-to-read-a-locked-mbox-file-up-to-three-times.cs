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
            const string mboxPath = "sample.mbox";

            // Ensure the MBOX file exists; create a minimal placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // Write a minimal MBOX separator to make the file valid.
                        byte[] separator = System.Text.Encoding.UTF8.GetBytes("From - \r\n");
                        placeholder.Write(separator, 0, separator.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            MboxStorageReader reader = null;
            const int maxAttempts = 3;
            int attempt = 0;
            bool readerCreated = false;

            // Retry policy for opening the MBOX reader.
            while (attempt < maxAttempts && !readerCreated)
            {
                attempt++;
                try
                {
                    reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());
                    readerCreated = true;
                }
                catch (IOException ioEx)
                {
                    Console.Error.WriteLine($"Attempt {attempt} - I/O error while opening MBOX: {ioEx.Message}");
                    if (attempt < maxAttempts)
                    {
                        Thread.Sleep(500); // Wait before retrying.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Attempt {attempt} - Unexpected error while opening MBOX: {ex.Message}");
                    return;
                }
            }

            if (!readerCreated || reader == null)
            {
                Console.Error.WriteLine("Failed to open the MBOX file after multiple attempts.");
                return;
            }

            using (reader)
            {
                while (true)
                {
                    MailMessage message = null;
                    try
                    {
                        message = reader.ReadNextMessage();
                        if (message == null)
                        {
                            break; // No more messages.
                        }

                        // Process the message (example: output subject).
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error reading a message: {ex.Message}");
                        break;
                    }
                    finally
                    {
                        if (message != null)
                        {
                            message.Dispose();
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
