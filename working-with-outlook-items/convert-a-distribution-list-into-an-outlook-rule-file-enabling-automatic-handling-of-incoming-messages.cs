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
            // Path to the distribution list MSG file
            string distListPath = "distributionList.msg";

            // Verify the file exists
            if (!File.Exists(distListPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(distListPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {distListPath}");
                return;
            }

            // Load the MSG file containing the distribution list
            using (MapiMessage msg = MapiMessage.Load(distListPath))
            {
                // Ensure the message is a distribution list
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The provided file is not a distribution list.");
                    return;
                }

                // Convert to a MapiDistributionList object
                using (MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem())
                {
                    // Placeholder connection details (replace with real values)
                    string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                    string username = "username@example.com";
                    string password = "password";

                    // Skip execution if placeholders are detected
                    if (serviceUrl.Contains("example.com") || username.Contains("username") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping client operations.");
                        return;
                    }

                    // Create an EWS client and ensure it is disposed properly
                    try
                    {
                        using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                        {
                            // Destination folder identifier (e.g., Inbox)
                            string destinationFolderId = "inbox";

                            // Create a rule for each member of the distribution list
                            foreach (MapiDistributionListMember member in distList.Members)
                            {
                                try
                                {
                                    InboxRule rule = InboxRule.CreateRuleMoveFrom(
                                        new MailAddress(member.EmailAddress),
                                        destinationFolderId);

                                    client.CreateInboxRule(rule);
                                    Console.WriteLine($"Created rule for {member.EmailAddress}");
                                }
                                catch (Exception ex)
                                {
                                    Console.Error.WriteLine($"Failed to create rule for {member.EmailAddress}: {ex.Message}");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to connect to Exchange service: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
