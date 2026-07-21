using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Mbox;
using Aspose.Words;
using Aspose.Words.Saving;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string outputDirectory = "output";

            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(
                        mboxPath,
                        "From sender@example.com Sat Jan 01 00:00:00 2022\r\n" +
                        "Subject: Placeholder\r\n" +
                        "From: sender@example.com\r\n" +
                        "To: recipient@example.com\r\n" +
                        "Date: Sat, 01 Jan 2022 00:00:00 +0000\r\n" +
                        "\r\n" +
                        "Placeholder body.\r\n");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            Directory.CreateDirectory(outputDirectory);

            using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
            {
                MailMessage message = mboxReader.ReadNextMessage();
                int messageIndex = 0;

                while (message != null)
                {
                    try
                    {
                        using (MailMessage currentMessage = message)
                        using (MemoryStream mhtmlStream = new MemoryStream())
                        {
                            currentMessage.Save(mhtmlStream, Aspose.Email.SaveOptions.DefaultMhtml);
                            mhtmlStream.Position = 0;

                            Document document = new Document(mhtmlStream);
                            ImageSaveOptions imageOptions = new ImageSaveOptions(Aspose.Words.SaveFormat.Tiff)
                            {
                                Resolution = 300,
                                UseAntiAliasing = true,
                                UseHighQualityRendering = true
                            };

                            string tiffPath = Path.Combine(outputDirectory, $"Message_{messageIndex}.tiff");
                            document.Save(tiffPath, imageOptions);
                            Console.WriteLine($"Converted message {messageIndex} to TIFF: {tiffPath}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to convert message {messageIndex}: {ex.Message}");
                    }

                    messageIndex++;
                    message = mboxReader.ReadNextMessage();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
