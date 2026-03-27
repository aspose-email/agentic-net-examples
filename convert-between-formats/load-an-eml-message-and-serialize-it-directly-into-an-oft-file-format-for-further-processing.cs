using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "input.eml";
            string oftPath = "output.oft";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            using (MailMessage mailMessage = MailMessage.Load(emlPath))
            {
                mailMessage.Save(oftPath, SaveOptions.DefaultOft);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
