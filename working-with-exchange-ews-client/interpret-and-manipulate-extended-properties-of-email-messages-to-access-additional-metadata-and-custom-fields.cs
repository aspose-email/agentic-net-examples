using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Fetch a message by its unique URI
                string messageUri = "https://exchange.example.com/EWS/MessageId";
                MailMessage message = client.FetchMessage(messageUri);

                // Display standard properties
                Console.WriteLine("Subject: " + (message.Subject ?? string.Empty));
                Console.WriteLine("From: " + (message.From?.DisplayName ?? string.Empty));

                // Display extended headers (custom metadata)
                foreach (string headerKey in message.Headers.AllKeys)
                {
                    Console.WriteLine($"{headerKey}: {message.Headers[headerKey]}");
                }
            }

            // Load a local MSG file to read custom MAPI properties
            string msgPath = "sample.msg";
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
            {
                Console.WriteLine("MSG Subject: " + (mapiMessage.Subject ?? string.Empty));

                // Retrieve custom properties collection
                MapiPropertyCollection customProps = mapiMessage.GetCustomProperties();
                foreach (KeyValuePair<long, MapiProperty> kvp in customProps)
                {
                    long tag = kvp.Key;
                    // Output the property tag; handling of the value depends on its type
                    Console.WriteLine($"Custom Property Tag: 0x{tag:X}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
