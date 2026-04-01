using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            string msgPath = "amp_message.msg";

            // Ensure the input MSG file exists; create a minimal placeholder if it does not.
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

                try
                {
                    using (AmpMessage placeholder = new AmpMessage())
                    {
                        placeholder.From = new MailAddress("sender@example.com", "Sender");
                        placeholder.To = new MailAddressCollection { new MailAddress("recipient@example.com", "Recipient") };
                        placeholder.Subject = "AMP Sample Message";
                        placeholder.AmpHtmlBody = "<!doctype html><html amp4email><head><meta charset='utf-8'></head><body><h1>Hello AMP</h1></body></html>";
                        placeholder.Save(msgPath);
                        Console.WriteLine($"Placeholder MSG created at '{msgPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and attempt to read AMP HTML content.
            try
            {
                using (MailMessage loadedMessage = MailMessage.Load(msgPath))
                {
                    AmpMessage ampMessage = loadedMessage as AmpMessage;
                    if (ampMessage != null)
                    {
                        Console.WriteLine("AMP HTML Body:");
                        Console.WriteLine(ampMessage.AmpHtmlBody);
                    }
                    else
                    {
                        Console.WriteLine("The loaded message does not contain AMP content.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
