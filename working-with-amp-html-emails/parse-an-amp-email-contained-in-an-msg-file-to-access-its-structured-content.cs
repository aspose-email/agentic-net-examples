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
            string msgPath = @"C:\Emails\amp_email.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file as a MailMessage, then cast to AmpMessage if possible
            using (MailMessage baseMessage = MailMessage.Load(msgPath))
            {
                AmpMessage ampMessage = baseMessage as AmpMessage;
                if (ampMessage == null)
                {
                    Console.Error.WriteLine("The loaded message is not an AMP message.");
                    return;
                }

                // Access structured AMP content
                Console.WriteLine($"Subject: {ampMessage.Subject}");
                Console.WriteLine($"From: {ampMessage.From}");
                Console.WriteLine($"To: {ampMessage.To}");
                Console.WriteLine($"AMP HTML Body:\n{ampMessage.AmpHtmlBody}");
                Console.WriteLine($"Plain Text Body:\n{ampMessage.Body}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
