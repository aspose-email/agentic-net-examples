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
            string inputMsgPath = "input.msg";
            string outputEmlPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(inputMsgPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load MSG file, convert to MailMessage, and save as EML
            using (MapiMessage mapiMessage = MapiMessage.Load(inputMsgPath))
            {
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    mailMessage.Save(outputEmlPath, SaveOptions.DefaultEml);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}