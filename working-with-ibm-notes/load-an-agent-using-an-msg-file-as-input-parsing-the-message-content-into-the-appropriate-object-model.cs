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
            string inputPath = "agent.msg";
            string outputDirectory = "output";
            string outputPath = Path.Combine(outputDirectory, "parsed.txt");

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Guard input file existence, create minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder Subject", "This is a placeholder body."))
                    {
                        MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                        placeholder.Save(inputPath, saveOptions);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and parse its content
            try
            {
                using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
                {
                    using (MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions()))
                    {
                        // Display parsed information
                        Console.WriteLine($"Subject: {mailMessage.Subject}");
                        Console.WriteLine($"From: {mailMessage.From}");
                        Console.WriteLine($"To: {mailMessage.To}");
                        Console.WriteLine($"Body: {mailMessage.Body}");

                        // Optionally write parsed details to a file
                        try
                        {
                            using (StreamWriter writer = new StreamWriter(outputPath, false))
                            {
                                writer.WriteLine($"Subject: {mailMessage.Subject}");
                                writer.WriteLine($"From: {mailMessage.From}");
                                writer.WriteLine($"To: {mailMessage.To}");
                                writer.WriteLine("Body:");
                                writer.WriteLine(mailMessage.Body);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error writing parsed output: {ex.Message}");
                            // Continue without aborting; parsed data already displayed on console
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading or parsing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
