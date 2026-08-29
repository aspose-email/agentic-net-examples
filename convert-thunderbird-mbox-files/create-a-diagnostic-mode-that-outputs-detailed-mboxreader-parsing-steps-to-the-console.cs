using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        // Path to the Thunderbird MBOX file.
        string mboxPath = "storage.mbox";

        // Verify that the MBOX file exists before attempting to read it.
        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
            return;
        }

        try
        {
            // Create a reader for the MBOX file. MboxLoadOptions can be customized if needed.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                Console.WriteLine($"Opened MBOX file: {mboxPath}");
                int messageIndex = 0;

                // Sequentially read each message using ReadNextMessage(out string) to obtain the "From" marker.
                while (true)
                {
                    string fromMarker;
                    MailMessage message = reader.ReadNextMessage(out fromMarker);

                    // When null is returned, we have reached the end of the storage.
                    if (message == null)
                        break;

                    messageIndex++;
                    Console.WriteLine($"--- Message #{messageIndex} ---");
                    Console.WriteLine($"From marker: {fromMarker}");
                    Console.WriteLine($"Subject    : {message.Subject}");
                    Console.WriteLine($"From       : {message.From}");
                    Console.WriteLine($"To         : {string.Join(", ", message.To)}");
                    Console.WriteLine($"Date       : {message.Date}");
                    Console.WriteLine($"CurrentDataSize (bytes read so far): {reader.CurrentDataSize}");
                    Console.WriteLine();
                }

                Console.WriteLine($"Finished processing {messageIndex} message(s).");
            }
        }
        catch (Exception ex)
        {
            // Surface any errors without crashing the application.
            Console.Error.WriteLine($"Error while processing MBOX: {ex.Message}");
        }
    }
}
