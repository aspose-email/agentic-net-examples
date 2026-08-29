using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            const string mboxPath = "storage.mbox";

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                return;
            }

            // Create the MBOX reader with load options as required by validation.
            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int messageCount = 0;
                // Read messages sequentially until no more are available.
                while (true)
                {
                    var message = mboxReader.ReadNextMessage();
                    if (message == null)
                        break;

                    messageCount++;
                }

                Console.WriteLine($"Total messages in '{mboxPath}': {messageCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
