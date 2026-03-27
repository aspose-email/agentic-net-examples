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
            string inputPath = "input.msg";
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            using (FileStream inputStream = File.OpenRead(inputPath))
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.Import(inputStream);

                Console.WriteLine("Subject: " + ampMessage.Subject);
                Console.WriteLine("AMP HTML Body:");
                Console.WriteLine(ampMessage.AmpHtmlBody ?? "(none)");

                string outputPath = "output.msg";
                using (FileStream outputStream = File.Create(outputPath))
                {
                    ampMessage.Save(outputStream, SaveOptions.DefaultMsg);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
