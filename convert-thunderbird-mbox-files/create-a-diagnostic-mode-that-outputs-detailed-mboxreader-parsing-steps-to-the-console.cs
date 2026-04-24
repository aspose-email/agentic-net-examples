using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "sample.mbox";

            // Guard file existence
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Create reader with load options inside try/catch
            try
            {
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;

                    while (true)
                    {
                        // Read next message with from‑marker information
                        MailMessage message = reader.ReadNextMessage(out string fromMarker);
                        if (message == null)
                            break;

                        using (message)
                        {
                            messageIndex++;
                            Console.WriteLine($"--- Message {messageIndex} ---");
                            Console.WriteLine($"From marker: {fromMarker}");
                            Console.WriteLine($"Subject: {message.Subject}");
                            Console.WriteLine($"From: {message.From}");
                            Console.WriteLine($"To: {message.To}");
                            Console.WriteLine();
                        }
                    }

                    Console.WriteLine($"Total messages processed: {messageIndex}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading MBOX file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
