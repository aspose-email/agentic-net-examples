using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string mailboxUri = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "your_username";
            string password = "your_password";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                mailboxUri.Contains("your") ||
                username.Contains("your") ||
                password.Contains("your"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Create EWS client.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // List existing distribution lists.
                ExchangeDistributionList[] dlists = null;
                try
                {
                    dlists = client.ListDistributionLists();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list distribution lists: {ex.Message}");
                    return;
                }

                if (dlists == null || dlists.Length == 0)
                {
                    Console.Error.WriteLine("No distribution lists found.");
                    return;
                }

                // Take the first distribution list.
                ExchangeDistributionList originalDl = dlists[0];
                Console.WriteLine($"Original Display Name: {originalDl.DisplayName}");

                // Fetch its members.
                MailAddressCollection members = null;
                try
                {
                    members = client.FetchDistributionList(originalDl);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to fetch members: {ex.Message}");
                    return;
                }

                // Delete the original distribution list (move to Deleted Items).
                try
                {
                    client.DeleteDistributionList(originalDl, false);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to delete original distribution list: {ex.Message}");
                    return;
                }

                // Create a new distribution list with updated display name.
                ExchangeDistributionList updatedDl = new ExchangeDistributionList
                {
                    DisplayName = originalDl.DisplayName + " - Updated"
                };

                string newDlId = null;
                try
                {
                    newDlId = client.CreateDistributionList(updatedDl, members);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create updated distribution list: {ex.Message}");
                    return;
                }

                // Verify the update by fetching the list again.
                ExchangeDistributionList[] refreshedLists = null;
                try
                {
                    refreshedLists = client.ListDistributionLists();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list distribution lists after update: {ex.Message}");
                    return;
                }

                // Find the newly created list.
                ExchangeDistributionList refreshedDl = null;
                foreach (var dl in refreshedLists)
                {
                    if (dl.Id == newDlId)
                    {
                        refreshedDl = dl;
                        break;
                    }
                }

                if (refreshedDl != null)
                {
                    Console.WriteLine($"Updated Display Name: {refreshedDl.DisplayName}");
                }
                else
                {
                    Console.Error.WriteLine("Updated distribution list not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
