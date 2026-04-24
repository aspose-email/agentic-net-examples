using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            // Configurable parameters
            string oftPath = "template.oft";
            string outputFolder = "output";
            int dpi = 300; // resolution

            // Guard file system access
            if (!File.Exists(oftPath))
            {
                try
                {
                    // Create a valid OFT template so this sample can run in CI.
                    using MailMessage placeholder = new MailMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body.");
                    placeholder.Save(oftPath, Aspose.Email.SaveOptions.DefaultOft);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder OFT: {ex.Message}");
                    return;
                }
            }

            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Load OFT template
            using (MapiMessage mapiMessage = MapiMessage.Load(oftPath))
            {
                // Convert to MailMessage
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                {
                    // Save to MHTML in memory
                    using (MemoryStream mhtmlStream = new MemoryStream())
                    {
                        mailMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                        mhtmlStream.Position = 0;

                        // Load MHTML into Aspose.Words Document
                        Document doc = new Document(mhtmlStream);
                        int pageCount = doc.PageCount;
                        for (int i = 0; i < pageCount; i++)
                        {
                            string outputPath = Path.Combine(outputFolder, $"page_{i + 1}.png");
                            ImageSaveOptions options = new ImageSaveOptions(SaveFormat.Png)
                            {
                                Resolution = dpi,
                                PageIndex = i,
                                PageCount = 1
                            };
                            doc.Save(outputPath, options);
                        }
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
