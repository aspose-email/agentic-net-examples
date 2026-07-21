using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange Web Services URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // -------------------- Create Distribution List --------------------
                ExchangeDistributionList newList = new ExchangeDistributionList();
                newList.DisplayName = "Sample Distribution List";

                MailAddressCollection initialMembers = new MailAddressCollection();
                initialMembers.Add(new MailAddress("alice@example.com"));
                initialMembers.Add(new MailAddress("bob@example.com"));

                // Create the list on the server; the method returns the list Id
                string listId = client.CreateDistributionList(newList, initialMembers);
                newList.Id = listId; // store the Id for later operations

                Console.WriteLine($"Created distribution list '{newList.DisplayName}' with Id: {newList.Id}");

                // -------------------- List All Distribution Lists --------------------
                ExchangeDistributionList[] allLists = client.ListDistributionLists();
                Console.WriteLine("\nExisting distribution lists:");
                foreach (ExchangeDistributionList dl in allLists)
                {
                    Console.WriteLine($"- {dl.DisplayName} (Id: {dl.Id})");
                }

                // -------------------- Fetch Members --------------------
                MailAddressCollection members = client.FetchDistributionList(newList);
                Console.WriteLine("\nCurrent members:");
                foreach (MailAddress addr in members)
                {
                    Console.WriteLine($"- {addr.Address}");
                }

                // -------------------- Add a Member --------------------
                MailAddressCollection membersToAdd = new MailAddressCollection();
                membersToAdd.Add(new MailAddress("charlie@example.com"));
                client.AddToDistributionList(newList, membersToAdd);
                Console.WriteLine("\nAdded member 'charlie@example.com'.");

                // Verify addition
                MailAddressCollection updatedMembers = client.FetchDistributionList(newList);
                Console.WriteLine("\nMembers after addition:");
                foreach (MailAddress addr in updatedMembers)
                {
                    Console.WriteLine($"- {addr.Address}");
                }

                // -------------------- Delete a Member --------------------
                MailAddressCollection membersToDelete = new MailAddressCollection();
                // Deleting 'bob@example.com' – the MailAddress must contain the Id; using address is sufficient for this example
                membersToDelete.Add(new MailAddress("bob@example.com"));
                client.DeleteFromDistributionList(newList, membersToDelete);
                Console.WriteLine("\nDeleted member 'bob@example.com'.");

                // Verify deletion
                MailAddressCollection afterDeletion = client.FetchDistributionList(newList);
                Console.WriteLine("\nMembers after deletion:");
                foreach (MailAddress addr in afterDeletion)
                {
                    Console.WriteLine($"- {addr.Address}");
                }

                // -------------------- Delete Distribution List --------------------
                client.DeleteDistributionList(newList, true);
                Console.WriteLine($"\nDeleted distribution list '{newList.DisplayName}' permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
