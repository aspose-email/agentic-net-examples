using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid unwanted network calls.
            if (serviceUrl.Contains("example") || username.Contains("example") || password == "password")
            {
                Console.Error.WriteLine("Please provide valid Exchange service URL and credentials.");
                return;
            }

            // Path to the distribution list MSG file.
            string dlPath = "distlist.msg";

            // Verify the file exists before attempting to load.
            if (!File.Exists(dlPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(dlPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Distribution list file not found: {dlPath}");
                return;
            }

            // Create the Exchange client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Load the MSG file as a MAPI message.
                    using (MapiMessage mapiMsg = MapiMessage.Load(dlPath))
                    {
                        // Ensure the message is a distribution list.
                        if (mapiMsg.SupportedType != MapiItemType.DistList)
                        {
                            Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                            return;
                        }

                        // Convert to a MapiDistributionList to access members.
                        MapiDistributionList distList = (MapiDistributionList)mapiMsg.ToMapiMessageItem();

                        // Create a rule for each member to move their messages to Inbox.
                        foreach (var member in distList.Members)
                        {
                            // member.EmailAddress may be null; skip such entries.
                            if (string.IsNullOrEmpty(member.EmailAddress))
                                continue;

                            MailAddress mailAddr = new MailAddress(member.EmailAddress);
                            // Create a rule that moves messages from this address to the Inbox folder.
                            InboxRule rule = InboxRule.CreateRuleMoveFrom(mailAddr, "Inbox");
                            client.CreateInboxRule(rule);
                        }

                        Console.WriteLine("Inbox rules created for distribution list members.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing distribution list: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
