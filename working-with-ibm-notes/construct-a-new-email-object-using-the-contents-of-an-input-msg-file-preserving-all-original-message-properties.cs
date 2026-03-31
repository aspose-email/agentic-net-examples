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
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder Body");
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the original MSG file
            MapiMessage originalMessage;
            try
            {
                originalMessage = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            using (originalMessage)
            {
                // Convert to MailMessage while preserving all properties
                MailMessage mailMessage;
                try
                {
                    mailMessage = originalMessage.ToMailMessage(new MailConversionOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error converting to MailMessage: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    // Save the new MailMessage to a new MSG file
                    try
                    {
                        MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                        mailMessage.Save(outputPath, saveOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving MailMessage: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
