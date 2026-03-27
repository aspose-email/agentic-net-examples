using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                // Convert to MailMessage
                MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                // Populate To recipients
                mailMessage.To.Add(new MailAddress("to1@example.com"));
                mailMessage.To.Add(new MailAddress("to2@example.com"));

                // Populate Cc recipients
                mailMessage.CC.Add(new MailAddress("cc1@example.com"));
                mailMessage.CC.Add(new MailAddress("cc2@example.com"));

                // Populate Bcc recipients
                mailMessage.Bcc.Add(new MailAddress("bcc1@example.com"));

                // Save the updated message to a new MSG file
                string outputPath = "updated.msg";
                using (MapiMessage updatedMapi = MapiMessage.FromMailMessage(mailMessage))
                {
                    updatedMapi.Save(outputPath);
                }

                Console.WriteLine($"Message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
