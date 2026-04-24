using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputDirectory = "InputMhtml";
            string outputDirectory = "OutputEml";

            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist.");
                return;
            }

            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            string[] mhtmlFiles = Directory.GetFiles(inputDirectory, "*.mht");

            foreach (string mhtmlPath in mhtmlFiles)
            {
                try
                {
                    if (!File.Exists(mhtmlPath))
                    {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(mhtmlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                        Console.Error.WriteLine($"File not found: {mhtmlPath}");
                        continue;
                    }

                    FileInfo fileInfo = new FileInfo(mhtmlPath);
                    using (MailMessage mailMessage = MailMessage.Load(mhtmlPath))
                    {
                        mailMessage.Date = fileInfo.LastWriteTime;

                        string emlFileName = Path.GetFileNameWithoutExtension(mhtmlPath) + ".eml";
                        string emlPath = Path.Combine(outputDirectory, emlFileName);

                        mailMessage.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{mhtmlPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
