using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "sample.msg";

            // Ensure the directory for the MSG file exists
            string? msgDir = Path.GetDirectoryName(msgPath);
            if (!string.IsNullOrEmpty(msgDir) && !Directory.Exists(msgDir))
            {
                Directory.CreateDirectory(msgDir);
            }

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file as a MapiMessage
            MapiMessage mapiMsg = MapiMessage.Load(msgPath);

            // Convert to a MailMessage
            MailMessage mailMsg = mapiMsg.ToMailMessage(new MailConversionOptions());

            // Attempt to treat the message as an AmpMessage
            if (mailMsg is AmpMessage ampMsg)
            {
                string ampBody = ampMsg.AmpHtmlBody;
                Console.WriteLine("AMP Body extracted via AmpMessage:");
                Console.WriteLine(ampBody);
                return;
            }

            // Fallback: search for an attachment with AMP content type
            foreach (MapiAttachment attachment in mapiMsg.Attachments)
            {
                if (attachment.FileName != null && attachment.FileName.EndsWith(".amp.html", StringComparison.OrdinalIgnoreCase))
                {
                    string tempPath = Path.Combine(Path.GetTempPath(), attachment.FileName);
                    try
                    {
                        attachment.Save(tempPath);
                        string ampContent = File.ReadAllText(tempPath);
                        Console.WriteLine("AMP Body extracted from attachment:");
                        Console.WriteLine(ampContent);
                    }
                    finally
                    {
                        if (File.Exists(tempPath))
                        {
                            File.Delete(tempPath);
                        }
                    }
                    return;
                }
            }

            Console.WriteLine("No AMP body found in the provided MSG file.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
