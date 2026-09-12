using Aspose.Email;
using Aspose.Email.Storage;
using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        // Expect input MBOX path and output PST path as arguments
        if (args.Length < 2)
        {
            Console.WriteLine("Usage: ConvertMboxToPst <input.mbox> <output.pst>");
            return;
        }

        string mboxPath = args[0];
        string pstPath = args[1];

        if (!File.Exists(mboxPath))
        {
            Console.WriteLine($"Input file not found: {mboxPath}");
            return;
        }

        try
        {
            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (var mboxStream = File.OpenRead(mboxPath))
            using (var pstStream = new MemoryStream())
            {
                MailStorageConverter.MboxToPst(mboxStream, pstStream);
                pstStream.Position = 0;

                using (var fileStream = File.Create(pstPath))
                {
                    pstStream.CopyTo(fileStream);
                }
            }

            Console.WriteLine($"Conversion successful. PST saved to: {pstPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion failed: {ex.Message}");
        }
    }
}
