using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            const string emlPath = "amp_email.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(emlPath))
            {
                try
                {
                    const string placeholderContent =
                        "From: test@example.com\r\n" +
                        "To: recipient@example.com\r\n" +
                        "Subject: Test AMP Email\r\n" +
                        "\r\n" +
                        "Hello, this is a placeholder EML file.";
                    File.WriteAllText(emlPath, placeholderContent);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Load the EML file into an AmpMessage instance
            using (FileStream stream = File.OpenRead(emlPath))
            {
                using (AmpMessage ampMessage = new AmpMessage())
                {
                    ampMessage.Import(stream);

                    Console.WriteLine("AMP HTML Body:");
                    Console.WriteLine(string.IsNullOrEmpty(ampMessage.AmpHtmlBody) ? "(none)" : ampMessage.AmpHtmlBody);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
