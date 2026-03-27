using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Retrieve the plain‑text body of the MSG email
                string plainText = mailMessage.Body ?? string.Empty;
                Console.WriteLine("Plain text body:");
                Console.WriteLine(plainText);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
