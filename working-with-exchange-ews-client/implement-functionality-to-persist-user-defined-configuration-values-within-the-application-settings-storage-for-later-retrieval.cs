using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the configuration file
            string configPath = "userconfig.txt";

            // Ensure the configuration file exists
            if (!File.Exists(configPath))
            {
                try
                {
                    File.WriteAllText(configPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create config file: {ex.Message}");
                    return;
                }
            }

            // Persist a user-defined value
            string valueToStore = "SampleValue";
            try
            {
                File.WriteAllText(configPath, valueToStore);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write config: {ex.Message}");
                return;
            }

            // Retrieve the persisted value
            string retrievedValue;
            try
            {
                retrievedValue = File.ReadAllText(configPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read config: {ex.Message}");
                return;
            }

            Console.WriteLine($"Retrieved config value: {retrievedValue}");

            // Example EWS client usage
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // List messages in the Inbox folder
                    var messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    Console.WriteLine($"Inbox contains {messages.Count} messages.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
