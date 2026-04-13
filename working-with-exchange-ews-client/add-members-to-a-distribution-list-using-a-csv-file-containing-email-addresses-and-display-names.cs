using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the CSV file containing display name and email address per line.
            string csvPath = "members.csv";

            // Ensure the CSV file exists; create a minimal placeholder if it does not.
            if (!File.Exists(csvPath))
            {
                try
                {
                    File.WriteAllText(csvPath, "John Doe,john.doe@example.com");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder CSV file: {ex.Message}");
                    return;
                }
            }

            // Load members from the CSV file.
            MailAddressCollection members = new MailAddressCollection();
            try
            {
                string[] lines = File.ReadAllLines(csvPath);
                foreach (string line in lines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] parts = line.Split(new char[] { ',' }, 2);
                    if (parts.Length != 2)
                        continue; // Skip malformed lines.

                    string displayName = parts[0].Trim();
                    string email = parts[1].Trim();

                    if (string.IsNullOrEmpty(email))
                        continue; // Skip entries without an email address.

                    MailAddress address = new MailAddress(email, displayName);
                    members.Add(address);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading CSV file: {ex.Message}");
                return;
            }

            // Create and connect the EWS client.
            IEWSClient client = null;
            try
            {
                // Replace the URL, username, and password with valid credentials.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or connect EWS client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly.
            using (client as IDisposable)
            {
                // Create a new distribution list (or retrieve an existing one).
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "My Distribution List";

                string distributionListId;
                try
                {
                    // Create the distribution list with no initial members.
                    distributionListId = client.CreateDistributionList(distributionList, new MailAddressCollection());
                    distributionList.Id = distributionListId;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create distribution list: {ex.Message}");
                    return;
                }

                // Add members from the CSV to the distribution list.
                try
                {
                    client.AddToDistributionList(distributionList, members);
                    Console.WriteLine("Members added to the distribution list successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add members to distribution list: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
