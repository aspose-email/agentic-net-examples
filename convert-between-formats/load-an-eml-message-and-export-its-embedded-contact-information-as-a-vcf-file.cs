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
            string outputFolder = "output";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            Directory.CreateDirectory(outputFolder);

            using (MailMessage message = MailMessage.Load(emlPath, new EmlLoadOptions()))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    if (attachment.Name != null && attachment.Name.EndsWith(".vcf", StringComparison.OrdinalIgnoreCase))
                    {
                        string vcfPath = Path.Combine(outputFolder, attachment.Name);
                        attachment.Save(vcfPath);
                        Console.WriteLine($"Saved contact: {vcfPath}");
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
