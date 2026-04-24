using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
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

            try
            {
                // Create the MBOX reader with load options
                using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageIndex = 0;

                    while (true)
                    {
                        MailMessage message = null;

                        // Read each message and handle possible corruption
                        try
                        {
                            message = mboxReader.ReadNextMessage();
                        }
                        catch (Exception readEx)
                        {
                            Console.Error.WriteLine($"Error reading message #{messageIndex + 1}: {readEx.Message}");
                            // Skip to next message
                            continue;
                        }

                        // End of file
                        if (message == null)
                            break;

                        // Process the message
                        using (message)
                        {
                            Console.WriteLine($"Message {++messageIndex}: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open MBOX reader: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
