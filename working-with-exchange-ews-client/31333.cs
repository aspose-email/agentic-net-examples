using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                MailMessage draft = new MailMessage();
                draft.From = "sender@example.com";
                draft.To.Add("recipient@example.com");
                draft.Subject = "Test Draft";
                draft.Body = "This is a draft email.";

                MapiMessage mapi = MapiMessage.FromMailMessage(draft);

                string draftUri = client.AppendMessage(client.MailboxInfo.DraftsUri, mapi, false);

                client.Send(client.FetchMessage(draftUri));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
