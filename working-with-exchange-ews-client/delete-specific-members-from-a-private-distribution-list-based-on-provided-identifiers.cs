using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with actual values)
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create and use the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve all private distribution lists
                        ExchangeDistributionList[] distributionLists = client.ListDistributionLists();

                        // Identify the distribution list to modify (replace with actual Id or DisplayName)
                        string targetListId = "YOUR_DISTRIBUTION_LIST_ID";

                        // Skip external calls when placeholder credentials are used
                        if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || targetListId.StartsWith("YOUR_"))
                        {
                            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                            return;
                        }

                        ExchangeDistributionList targetList = null;
                        foreach (ExchangeDistributionList list in distributionLists)
                        {
                            if (list.Id == targetListId)
                            {
                                targetList = list;
                                break;
                            }
                        }

                        if (targetList == null)
                        {
                            Console.Error.WriteLine("Distribution list not found.");
                            return;
                        }

                        // Prepare the collection of members to delete
                        MailAddressCollection membersToDelete = new MailAddressCollection();
                        // Add members by their email addresses (replace with actual addresses)
                        membersToDelete.Add(new MailAddress("member1@example.com"));
                        membersToDelete.Add(new MailAddress("member2@example.com"));

                        // Delete the specified members from the distribution list
                        client.DeleteFromDistributionList(targetList, membersToDelete);
                        Console.WriteLine("Specified members have been removed from the distribution list.");
                    }
                    catch (Exception ex)
                    {
                        // Handle errors related to EWS operations
                        Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle errors related to client creation or authentication
                Console.Error.WriteLine("Failed to initialize EWS client: " + ex.Message);
            }
        }
    }
}
