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

            // Ensure the MSG file exists; create a minimal placeholder if it does not.
            if (!File.Exists(msgPath))
            {
                using (MapiMessage placeholder = new MapiMessage(
                    "sender@example.com",
                    "receiver@example.com",
                    "Placeholder Subject",
                    "Placeholder body"))
                {
                    placeholder.Save(msgPath);
                }
            }

            // Load the MSG file as a MailMessage and output its subject.
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                Console.WriteLine("Subject: " + message.Subject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
