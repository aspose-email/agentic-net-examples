using System;
using System.IO;
using Aspose.Email;

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

            try
            {
                using (MailMessage message = MailMessage.Load(msgPath))
                {
                    string subject = message.Subject ?? string.Empty;
                    Console.WriteLine("Subject: " + subject);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error loading MSG file: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
