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
            string msgPath = "amp_message.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(msgPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // If the MSG file does not exist, create a minimal placeholder AMP message
            if (!File.Exists(msgPath))
            {
                using (AmpMessage placeholder = new AmpMessage())
                {
                    placeholder.From = new MailAddress("placeholder@example.com");
                    placeholder.To.Add(new MailAddress("recipient@example.com"));
                    placeholder.Subject = "Placeholder AMP Message";
                    placeholder.Body = "Plain text fallback.";
                    placeholder.AmpHtmlBody = "<amp-email></amp-email>";
                    placeholder.Save(msgPath);
                }
            }

            // Load the existing MSG file as a MailMessage
            using (MailMessage loadedMessage = MailMessage.Load(msgPath))
            {
                // Attempt to cast to AmpMessage to access AMP-specific properties
                AmpMessage ampMessage = loadedMessage as AmpMessage;
                if (ampMessage != null)
                {
                    Console.WriteLine("AMP HTML Body:");
                    Console.WriteLine(ampMessage.AmpHtmlBody);
                }
                else
                {
                    Console.WriteLine("The loaded message does not contain AMP content.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
