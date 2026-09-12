// Author: Aspose.Email conversion sample
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
            // Define file paths
            string emlInputPath = "input.eml";
            string msgOutputPath = "output.msg";
            
            string outputDir = Path.GetDirectoryName(msgOutputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }
string msgInputPath = "input.msg";
            string emlOutputPath = "output.eml";

            // Convert EML to MSG
            if (!File.Exists(emlInputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlInputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"EML input file not found: {emlInputPath}");
            }
            else
            {
                try
                {
                    EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
                    {
                        PreserveTnefAttachments = true,
                        PreserveEmbeddedMessageFormat = true
                    };
                    using (MailMessage emlMessage = MailMessage.Load(emlInputPath, emlLoadOptions))
                    {
                        emlMessage.Save(msgOutputPath, SaveOptions.DefaultMsg);
                    }
                    Console.WriteLine($"Converted EML to MSG: {msgOutputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error converting EML to MSG: {ex.Message}");
                    return;
                }
            }

            // Convert MSG to EML
            if (!File.Exists(msgInputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgInputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"MSG input file not found: {msgInputPath}");
            }
            else
            {
                try
                {
                    MapiMessage mapiMsg = MapiMessage.Load(msgInputPath);
                    MailConversionOptions convOptions = new MailConversionOptions();
                    using (MailMessage mailMsg = mapiMsg.ToMailMessage(convOptions))
                    {
                        mailMsg.Save(emlOutputPath);
                    }
                    Console.WriteLine($"Converted MSG to EML: {emlOutputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error converting MSG to EML: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
