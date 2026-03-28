using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define the supported file extensions for MSG import capability
            List<string> supportedExtensions = new List<string>
            {
                "eml",
                "html",
                "mhtml",
                "msg",
                "dat"
            };

            foreach (string extension in supportedExtensions)
            {
                string filePath = $"sample.{extension}";
                // Guard file existence
                if (!File.Exists(filePath))
                {
                    Console.Error.WriteLine($"File not found: {filePath}");
                    continue;
                }

                try
                {
                    // Load the message using the appropriate LoadOptions when required
                    MailMessage message;
                    switch (extension)
                    {
                        case "eml":
                            message = MailMessage.Load(filePath, new EmlLoadOptions());
                            break;
                        case "html":
                            message = MailMessage.Load(filePath, new HtmlLoadOptions());
                            break;
                        case "mhtml":
                            message = MailMessage.Load(filePath, new MhtmlLoadOptions());
                            break;
                        case "msg":
                            message = MailMessage.Load(filePath, new MsgLoadOptions());
                            break;
                        case "dat":
                            // DAT files can be loaded without specific options
                            message = MailMessage.Load(filePath);
                            break;
                        default:
                            // Should never reach here due to the predefined list
                            continue;
                    }

                    using (MailMessage disposableMessage = message)
                    {
                        Console.WriteLine($"Loaded '{filePath}' successfully. Subject: {disposableMessage.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load '{filePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
