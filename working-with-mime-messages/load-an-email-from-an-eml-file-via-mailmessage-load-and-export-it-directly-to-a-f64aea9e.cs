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
            string msgPath = "output.msg";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Input file not found: {emlPath}");
                return;
            }

            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Export the loaded EML message to MSG format
                message.Save(msgPath, SaveOptions.DefaultMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
