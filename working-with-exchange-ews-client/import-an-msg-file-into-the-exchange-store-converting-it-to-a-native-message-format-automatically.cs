using Aspose.Email.Clients.Exchange.WebService;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file to import
            string msgFilePath = "sample.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgFilePath}");
                return;
            }

            // Load the MSG file into a MailMessage object
            MailMessage mailMessage;
            try
            {
                mailMessage = MailMessage.Load(msgFilePath);
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
                return;
            }

            // Exchange server connection details
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            ICredentials credentials = new NetworkCredential("username", "password");

            // Create and use the Exchange client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(exchangeUri, credentials))
                {
                    try
                    {
                        // Append the message to the default Inbox folder
                        string messageUri = client.AppendMessage(mailMessage);
                        Console.WriteLine($"Message imported successfully. URI: {messageUri}");
                    }
                    catch (Exception appendEx)
                    {
                        Console.Error.WriteLine($"Failed to import message: {appendEx.Message}");
                    }
                }
            }
            catch (Exception clientEx)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {clientEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
