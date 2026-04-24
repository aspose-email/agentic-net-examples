using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            string oftPath = "template.oft";
            string mhtmlPath = "output.mhtml";
            string xpsPath = "output.xps";

            if (!File.Exists(oftPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Sample OFT Template",
                        "This is a placeholder OFT template."))
                    {
                        placeholder.SaveAsTemplate(oftPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OFT: {ex.Message}");
                    return;
                }
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(oftPath))
            using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
            {
                mailMessage.Save(mhtmlPath, Aspose.Email.SaveOptions.DefaultMhtml);
            }

            Document document = new Document(mhtmlPath);
            document.Save(xpsPath, Aspose.Words.SaveFormat.Xps);
            Console.WriteLine($"OFT file successfully converted to XPS: {xpsPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
