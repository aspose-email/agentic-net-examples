using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("sender@example.com", "receiver@example.com", "Placeholder Subject", "Placeholder Body"))
                    {
                        placeholder.Save(msgPath);
                        Console.WriteLine("Created placeholder MSG file at: " + msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder MSG file: " + ex.Message);
                    return;
                }
            }

            // Load the MSG file and extract the AMP HTML body.
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
                Console.Error.WriteLine("Error processing MSG file: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}