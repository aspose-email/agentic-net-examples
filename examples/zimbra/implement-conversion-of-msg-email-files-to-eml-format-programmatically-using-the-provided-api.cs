using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string msgFilePath = "input.msg";
            string emlFilePath = "output.eml";

            if (!File.Exists(msgFilePath))
            {
                Console.Error.WriteLine($"Error: File not found – {msgFilePath}");
                return;
            }

            // Load the MSG file as a MailMessage (Aspose.Email can auto-detect the format)
            using (Aspose.Email.MailMessage mailMessage = Aspose.Email.MailMessage.Load(msgFilePath))
            {
                // Save the message in EML format
                mailMessage.Save(emlFilePath, Aspose.Email.SaveOptions.DefaultEml);
                Console.WriteLine($"Conversion successful: \"{msgFilePath}\" → \"{emlFilePath}\"");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}