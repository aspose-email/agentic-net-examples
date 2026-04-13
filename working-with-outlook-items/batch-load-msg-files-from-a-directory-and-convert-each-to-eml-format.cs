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
            string inputDir = "MsgFiles";
            string outputDir = "EmlOutput";

            if (!Directory.Exists(inputDir))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDir}");
                return;
            }

            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputDir, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate MSG files: {ex.Message}");
                return;
            }

            foreach (string msgPath in msgFiles)
            {
                if (!File.Exists(msgPath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {msgPath}");
                    continue;
                }

                try
                {
                    using (MapiMessage mapiMsg = MapiMessage.Load(msgPath))
                    {
                        MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());
                        string emlFileName = Path.GetFileNameWithoutExtension(msgPath) + ".eml";
                        string emlPath = Path.Combine(outputDir, emlFileName);
                        mailMsg.Save(emlPath);
                        Console.WriteLine($"Converted: {msgPath} -> {emlPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{msgPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
