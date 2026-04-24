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
            string oftPath = "template.oft";
            string mhtmlPath = "output.mhtml";

            if (!File.Exists(oftPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(oftPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {oftPath}");
                return;
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(oftPath))
            {
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    MhtSaveOptions saveOptions = new MhtSaveOptions
                    {
                        // Ensure linked resources are embedded in the MHTML output
                        ExtractHTMLBodyResourcesAsAttachments = false
                    };

                    mailMessage.Save(mhtmlPath, saveOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
