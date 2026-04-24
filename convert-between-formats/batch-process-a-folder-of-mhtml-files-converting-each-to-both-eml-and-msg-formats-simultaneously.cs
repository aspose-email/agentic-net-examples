using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputFolder = "MhtmlFiles";
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            string outputFolder = "Converted";
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            string[] mhtmlFiles = Directory.GetFiles(inputFolder, "*.mhtml");
            foreach (string mhtmlPath in mhtmlFiles)
            {
                try
                {
                    MhtmlLoadOptions loadOptions = new MhtmlLoadOptions();
                    using (MailMessage message = MailMessage.Load(mhtmlPath, loadOptions))
                    {
                        string baseName = Path.GetFileNameWithoutExtension(mhtmlPath);
                        string emlPath = Path.Combine(outputFolder, baseName + ".eml");
                        string msgPath = Path.Combine(outputFolder, baseName + ".msg");

                        EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                        message.Save(emlPath, emlSaveOptions);

                        message.Save(msgPath, SaveOptions.DefaultMsg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert '{mhtmlPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
