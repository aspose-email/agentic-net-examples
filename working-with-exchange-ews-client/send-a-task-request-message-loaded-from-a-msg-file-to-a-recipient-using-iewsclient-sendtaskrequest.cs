using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Input parameters
            string msgFilePath = "TaskRequest.msg";
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Verify MSG file exists
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

                Console.Error.WriteLine($"Input file not found: {msgFilePath}");
                return;
            }

            // Load the task request message from MSG file
            MailMessage taskRequestMessage;
            try
            {
                taskRequestMessage = MailMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Send the task request message
            try
            {
                client.Send(taskRequestMessage);
                Console.WriteLine("Task request sent successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send task request: {ex.Message}");
            }
            finally
            {
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
