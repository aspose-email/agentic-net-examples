using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath;
            if (args.Length > 0)
            {
                inputPath = args[0];
            }
            else
            {
                Console.Write("Enter the full path to the Outlook MSG file: ");
                inputPath = Console.ReadLine();
            }

            if (string.IsNullOrEmpty(inputPath))
            {
                Console.Error.WriteLine("Input path is empty.");
                return;
            }

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {inputPath}");
                return;
            }

            string outputPath = Path.ChangeExtension(inputPath, ".eml");
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MapiMessage mapMessage = MapiMessage.Load(inputPath))
            {
                MailMessage mailMessage = mapMessage.ToMailMessage(new MailConversionOptions());
                mailMessage.Save(outputPath);
                Console.WriteLine($"Message saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
