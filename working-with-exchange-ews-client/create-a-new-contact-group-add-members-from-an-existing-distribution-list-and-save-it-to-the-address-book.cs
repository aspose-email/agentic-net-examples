using Aspose.Email.PersonalInfo;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Exchange server connection settings
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Existing distribution list identifier (replace with a real Id)
                    string existingListId = "existing-list-id";


                    // Skip external calls when placeholder credentials are used
                    if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Prepare the existing distribution list object
                    ExchangeDistributionList existingList = new ExchangeDistributionList();
                    existingList.Id = existingListId;

                    // Fetch members of the existing distribution list
                    MailAddressCollection existingMembers = client.FetchDistributionList(existingList);

                    // Create a new distribution list (contact group)
                    ExchangeDistributionList newList = new ExchangeDistributionList();
                    newList.DisplayName = "New Contact Group";

                    // Create the new distribution list with members copied from the existing list
                    string newListId = client.CreateDistributionList(newList, existingMembers);

                    Console.WriteLine("New distribution list created with Id: " + newListId);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
