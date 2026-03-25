using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine("Input file not found: " + msgPath);
                return;
            }

            try
            {
                using (FileStream fileStream = File.OpenRead(msgPath))
                {
                    using (AmpMessage ampMessage = new AmpMessage())
                    {
                        ampMessage.Import(fileStream);
                        string ampHtmlBody = ampMessage.AmpHtmlBody ?? string.Empty;
                        Console.WriteLine("AMP HTML Body:");
                        Console.WriteLine(ampHtmlBody);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing the MSG file: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}