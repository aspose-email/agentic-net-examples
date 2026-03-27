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
            string msgFilePath = "sample.msg";
            string mhtFilePath = "output.mht";

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(msgFilePath))
            {
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    mailMessage.Save(mhtFilePath, SaveOptions.DefaultMhtml);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
