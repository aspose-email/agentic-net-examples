using System;
using System.Net;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credential))
            {
                // Create a simple MailMessage
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = "sender@example.com";
                    mail.To.Add("receiver@example.com");
                    mail.Subject = "Test with custom property";
                    mail.Body = "Hello, this message contains a custom extended property.";

                    // Convert MailMessage to MapiMessage to add custom property
                    using (MapiMessage mapiMsg = MapiMessage.FromMailMessage(mail))
                    {
                        // Add a custom Unicode property
                        byte[] customData = Encoding.UTF8.GetBytes("CustomValue");
                        mapiMsg.AddCustomProperty(MapiPropertyType.PT_UNICODE, customData, "MyCustomProp");

                        // Convert back to MailMessage for sending via EWS
                        using (MailMessage mailWithProp = mapiMsg.ToMailMessage(new MailConversionOptions()))
                        {
                            client.Send(mailWithProp);
                            Console.WriteLine("Message sent successfully.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
