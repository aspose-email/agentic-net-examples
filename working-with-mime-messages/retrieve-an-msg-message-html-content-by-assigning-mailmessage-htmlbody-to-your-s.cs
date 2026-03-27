using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "message.msg";

            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            try
            {
                using (MailMessage mailMessage = MailMessage.Load(msgPath))
                {
                    string htmlContent = mailMessage.HtmlBody;
                    Console.WriteLine(htmlContent);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading message: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
