using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            string tgzPath = "mailbox.tgz";
            string outputDirectory = "ExportedMessages";

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (FileStream fileStream = File.OpenRead(tgzPath))
            {
                using (TgzReader tgzReader = new TgzReader(fileStream))
                {
                    try
                    {
                        tgzReader.ExportTo(outputDirectory);
                    }
                    catch (Exception exportEx)
                    {
                        Console.Error.WriteLine($"Error during export: {exportEx.Message}");
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