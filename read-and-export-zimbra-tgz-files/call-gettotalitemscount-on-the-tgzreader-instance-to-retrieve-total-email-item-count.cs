using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            string tgzPath = "mailbox.tgz";

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"File not found: {tgzPath}");
                return;
            }

            using (TgzReader reader = new TgzReader(tgzPath))
            {
                int totalItemsCount = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItemsCount}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
