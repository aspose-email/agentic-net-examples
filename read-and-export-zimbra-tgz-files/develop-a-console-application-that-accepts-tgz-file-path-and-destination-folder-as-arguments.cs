using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args == null || args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <program> <tgzFilePath> <destinationFolder>");
                return;
            }

            string tgzFilePath = args[0];
            string destinationFolder = args[1];

            if (string.IsNullOrWhiteSpace(tgzFilePath))
            {
                Console.Error.WriteLine("TGZ file path is empty.");
                return;
            }

            if (string.IsNullOrWhiteSpace(destinationFolder))
            {
                Console.Error.WriteLine("Destination folder path is empty.");
                return;
            }

            if (!File.Exists(tgzFilePath))
            {
                Console.Error.WriteLine($"TGZ file not found: {tgzFilePath}");
                return;
            }

            try
            {
                if (!Directory.Exists(destinationFolder))
                {
                    Directory.CreateDirectory(destinationFolder);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create destination folder: {dirEx.Message}");
                return;
            }

            try
            {
                using (TgzReader reader = new TgzReader(tgzFilePath))
                {
                    reader.ExportTo(destinationFolder);
                }

                Console.WriteLine("Extraction completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during extraction: {ex.Message}");
                return;
            }
        }
        catch (Exception e)
        {
            Console.Error.WriteLine($"Unexpected error: {e.Message}");
        }
    }
}
