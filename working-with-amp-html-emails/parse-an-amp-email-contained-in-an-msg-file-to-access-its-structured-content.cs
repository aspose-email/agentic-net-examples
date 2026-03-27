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
            string msgPath = "message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            try
            {
                using (FileStream fileStream = File.OpenRead(msgPath))
                {
                    using (AmpMessage ampMessage = new AmpMessage())
                    {
                        ampMessage.Import(fileStream);

                        Console.WriteLine("Subject: " + ampMessage.Subject);
                        Console.WriteLine("From: " + (ampMessage.From?.DisplayName ?? ampMessage.From?.Address ?? "Unknown"));
                        Console.WriteLine("AMP HTML Body:");
                        Console.WriteLine(ampMessage.AmpHtmlBody ?? "(none)");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing the MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
