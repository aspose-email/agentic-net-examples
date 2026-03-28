using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string emlPath = "input.eml";
            string ostPath = "output.ost";

            if (!File.Exists(emlPath))
            {
                try
                {
                    string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML.";
                    File.WriteAllText(emlPath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
                    return;
                }
            }

            string outputDir = Path.GetDirectoryName(ostPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            if (!File.Exists(ostPath))
            {
                try
                {
                    PersonalStorage.Create(ostPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OST: {ex.Message}");
                    return;
                }
            }

            using (PersonalStorage storage = PersonalStorage.FromFile(ostPath))
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(message);
                storage.RootFolder.AddMessage(mapiMessage);
                Console.WriteLine("Message added to OST successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
