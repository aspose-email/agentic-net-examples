using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Mailbox URI and credentials (replace with real values)
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Name of the distribution list to modify
                    string targetDlName = "Sample Distribution List";

                    // Retrieve all private distribution lists
                    ExchangeDistributionList[] dlList = client.ListDistributionLists();

                    // Locate the desired distribution list
                    ExchangeDistributionList targetDl = null;
                    foreach (ExchangeDistributionList dl in dlList)
                    {
                        if (dl.DisplayName == targetDlName)
                        {
                            targetDl = dl;
                            break;
                        }
                    }

                    if (targetDl == null)
                    {
                        Console.WriteLine($"Distribution list '{targetDlName}' not found.");
                        return;
                    }

                    // Fetch current members of the distribution list
                    MailAddressCollection members = client.FetchDistributionList(targetDl);

                    // Display name of the member to remove
                    string memberDisplayName = "John Doe";


                    // Skip external calls when placeholder credentials are used
                    if (mailboxUri.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                        return;
                    }

                    // Find the member with the specified display name
                    MailAddress memberToRemove = null;
                    foreach (MailAddress addr in members)
                    {
                        if (addr.DisplayName == memberDisplayName)
                        {
                            memberToRemove = addr;
                            break;
                        }
                    }

                    if (memberToRemove == null)
                    {
                        Console.WriteLine($"Member '{memberDisplayName}' not found in distribution list.");
                        return;
                    }

                    // Prepare collection with the member to delete
                    MailAddressCollection deleteCollection = new MailAddressCollection();
                    deleteCollection.Add(memberToRemove);

                    // Delete the member from the distribution list
                    client.DeleteFromDistributionList(targetDl, deleteCollection);
                    Console.WriteLine($"Member '{memberDisplayName}' removed from distribution list.");

                    // Confirm removal by fetching the list again
                    MailAddressCollection updatedMembers = client.FetchDistributionList(targetDl);
                    bool stillExists = false;
                    foreach (MailAddress addr in updatedMembers)
                    {
                        if (addr.DisplayName == memberDisplayName)
                        {
                            stillExists = true;
                            break;
                        }
                    }

                    Console.WriteLine(stillExists ? "Removal failed." : "Removal confirmed.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
