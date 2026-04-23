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

            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                while (true)
                {
                    MailMessage message = reader.ReadNextMessage();
                    if (message == null)
                        break;

                    using (message)
                    {
                        Console.WriteLine($"From: {message.From}, Date: {message.Date}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
