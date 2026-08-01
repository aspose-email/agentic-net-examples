using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    // Author: Sample demonstrating async‑style workflow with Aspose.Email EWS client (synchronous API used safely)
    static void Main()
    {
        try
        {
            // ----- Configuration -----
            const string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("user@example.com", "password");
            const string msgPath = "sample.msg";

            // ----- Ensure placeholder MSG file exists -----
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Test Subject",
                        "Test Body"))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // ----- Load MSG as MapiMessage -----
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // ----- Create EWS client -----
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, credentials);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Ensure client is disposed at the end
            try
            {
                // ----- Append message as draft -----
                string draftsFolderUri = client.MailboxInfo.DraftsUri;
                string draftMessageUri;
                try
                {
                    draftMessageUri = client.AppendMessage(draftsFolderUri, mapiMessage, true);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append draft message: {ex.Message}");
                    return;
                }

                // ----- Fetch the draft as MailMessage and send it -----
                MailMessage mailMessage;
                try
                {
                    mailMessage = client.FetchMessage(draftMessageUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch draft message: {ex.Message}");
                    return;
                }

                using (mailMessage)
                {
                    try
                    {
                        client.Send(mailMessage);
                        Console.WriteLine("Draft message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                    }
                }
            }
            finally
            {
                // Dispose the client if it implements IDisposable
                if (client is IDisposable disposableClient)
                {
                    disposableClient.Dispose();
                }
            }

            // Dispose the loaded MapiMessage
            mapiMessage.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
