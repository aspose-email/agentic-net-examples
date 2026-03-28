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

            if (!File.Exists(inputMsgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(inputMsgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            string outputDir = Path.GetDirectoryName(outputEmlPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create directory – {outputDir}. {ex.Message}");
                    return;
                }
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
                {
                    MailConversionOptions conversionOptions = new MailConversionOptions();
                    using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                    {
                        SaveOptions emlSaveOptions = SaveOptions.DefaultEml;
                        mail.Save(outputEmlPath, emlSaveOptions);
                    }
                }

                Console.WriteLine($"MSG file converted to EML successfully: {outputEmlPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during conversion: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
