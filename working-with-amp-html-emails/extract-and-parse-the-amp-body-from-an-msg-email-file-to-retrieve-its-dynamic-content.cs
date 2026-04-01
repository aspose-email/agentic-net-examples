using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "input.msg";
            string outputPath = "amp_body.html";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input MSG file not found: {msgPath}");
                return;
            }

            // Load the MSG as an AmpMessage
            using (AmpMessage ampMessage = (AmpMessage)MailMessage.Load(msgPath))
            {
                string ampHtmlBody = ampMessage.AmpHtmlBody;

                if (string.IsNullOrEmpty(ampHtmlBody))
                {
                    Console.WriteLine("The message does not contain an AMP body.");
                    return;
                }

                try
                {
                    // Ensure output directory exists
                    string outputDirectory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }

                    File.WriteAllText(outputPath, ampHtmlBody);
                    Console.WriteLine($"AMP body extracted and saved to: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write AMP body to file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
