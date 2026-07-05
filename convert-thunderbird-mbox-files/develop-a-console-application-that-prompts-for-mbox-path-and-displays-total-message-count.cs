using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        Console.Write("Enter the full path to the MBOX file: ");
        string mboxPath = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(mboxPath))
        {
            Console.Error.WriteLine("No path was provided.");
            return;
        }

        if (!File.Exists(mboxPath))
        {
            Console.Error.WriteLine($"File not found: {mboxPath}");
            return;
        }

        try
        {
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                int totalMessageCount = 0;
                while (true)
                {
                    var message = reader.ReadNextMessage();
                    if (message == null)
                        break;
                    totalMessageCount++;
                }

                Console.WriteLine($"Total messages in '{Path.GetFileName(mboxPath)}': {totalMessageCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing MBOX file: {ex.Message}");
        }
    }
}
