using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = @"c:\outlookmessage.msg";

            // Verify that the MSG file exists before attempting to load it.
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage instance.
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                // Retrieve the sender's email address safely.
                string senderEmail = message.From?.Address ?? string.Empty;
                Console.WriteLine($"Sender Email: {senderEmail}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
