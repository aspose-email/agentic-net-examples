using System;
using System.IO;
using Aspose.Email;

namespace AsposeEmailExample
{
    // Author: Generated example for retrieving HTML body from an MSG file using Aspose.Email.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define the path to the MSG file.
                string msgPath = "message.msg";

                // Verify that the input file exists.
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

                    Console.Error.WriteLine($"Input file not found: {msgPath}");
                    return;
                }

                // Load the MSG file into a MailMessage instance.
                using (MailMessage mailMessage = MailMessage.Load(msgPath))
                {
                    // Retrieve the HTML body of the message.
                    string htmlBody = mailMessage.HtmlBody;

                    // Output the HTML content (or indicate if empty).
                    if (!string.IsNullOrEmpty(htmlBody))
                    {
                        Console.WriteLine("HTML Body:");
                        Console.WriteLine(htmlBody);
                    }
                    else
                    {
                        Console.WriteLine("The message does not contain an HTML body.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any unexpected errors without crashing the application.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
