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
                // Placeholder connection settings
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls during CI
                if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                    return;
                }

                // Create and connect the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    try
                    {
                        // Identify the existing distribution list (replace with a real Id)
                        ExchangeDistributionList distributionList = new ExchangeDistributionList();
                        distributionList.Id = "distribution-list-id";

                        // Fetch current members of the distribution list
                        MailAddressCollection existingMembers = client.FetchDistributionList(distributionList);

                        // Prepare the list of contacts to add
                        MailAddressCollection contactsToAdd = new MailAddressCollection();
                        contactsToAdd.Add(new MailAddress("alice@example.com"));
                        contactsToAdd.Add(new MailAddress("bob@example.com"));
                        contactsToAdd.Add(new MailAddress("carol@example.com"));

                        // Build a collection containing only non‑duplicate members
                        MailAddressCollection membersToAdd = new MailAddressCollection();
                        foreach (MailAddress newMember in contactsToAdd)
                        {
                            bool alreadyExists = false;
                            foreach (MailAddress existing in existingMembers)
                            {
                                if (string.Equals(existing.Address, newMember.Address, StringComparison.OrdinalIgnoreCase))
                                {
                                    alreadyExists = true;
                                    break;
                                }
                            }
                            if (!alreadyExists)
                            {
                                membersToAdd.Add(newMember);
                            }
                        }

                        // Add the non‑duplicate members to the distribution list
                        if (membersToAdd.Count > 0)
                        {
                            client.AddToDistributionList(distributionList, membersToAdd);
                            Console.WriteLine("Added {0} new member(s) to the distribution list.", membersToAdd.Count);
                        }
                        else
                        {
                            Console.WriteLine("No new members to add; all contacts are already members.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Exchange operation failed: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
