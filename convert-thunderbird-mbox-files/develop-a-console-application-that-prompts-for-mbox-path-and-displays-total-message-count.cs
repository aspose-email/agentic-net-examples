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
            Console.Write("Enter MBOX file path: ");
            string mboxPath = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(mboxPath) || !File.Exists(mboxPath))
            {
                Console.Error.WriteLine("MBOX file not found.");
                return;
            }

            try
            {
                using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                {
                    int messageCount = 0;
                    while (true)
                    {
                        MailMessage message = reader.ReadNextMessage();
                        if (message == null)
                            break;

                        using (message)
                        {
                            messageCount++;
                        }
                    }

                    Console.WriteLine($"Total messages in MBOX: {messageCount}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
