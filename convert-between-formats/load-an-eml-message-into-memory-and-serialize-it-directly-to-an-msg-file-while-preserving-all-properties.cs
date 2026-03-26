using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string emlPath = "input.eml";
            string msgPath = "output.msg";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            using (FileStream emlStream = File.OpenRead(emlPath))
            {
                // Load the EML into a MailMessage
                using (MailMessage mailMessage = MailMessage.Load(emlStream))
                {
                    // Convert MailMessage to MapiMessage preserving all properties
                    using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(mailMessage))
                    {
                        // Save the MapiMessage as MSG
                        mapiMessage.Save(msgPath);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}