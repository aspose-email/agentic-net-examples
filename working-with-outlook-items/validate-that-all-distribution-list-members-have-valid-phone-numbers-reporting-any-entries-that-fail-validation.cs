using Aspose.Email.Clients.Exchange;
using System;
using System.Text.RegularExpressions;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against running with placeholder credentials.
            if (serviceUrl.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder Exchange credentials detected. Skipping execution.");
                return;
            }

            // Create the Exchange Web Services client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Define the distribution list to inspect.
                ExchangeDistributionList distributionList = new ExchangeDistributionList();
                distributionList.DisplayName = "Sample Distribution List";

                // Fetch members of the distribution list.
                MailAddressCollection members = client.FetchDistributionList(distributionList);

                // Regular expression for a simple international phone number validation.
                Regex phoneRegex = new Regex(@"^\+?\d{10,15}$");

                bool anyInvalid = false;

                // Iterate through members and validate phone numbers.
                foreach (MailAddress member in members)
                {
                    // For demonstration, assume the phone number is stored in the DisplayName.
                    // In real scenarios, retrieve the contact and its PhoneNumbers collection.
                    string phoneNumber = member.DisplayName?.Trim() ?? string.Empty;

                    if (!phoneRegex.IsMatch(phoneNumber))
                    {
                        anyInvalid = true;
                        Console.WriteLine($"Invalid phone number for member '{member.Address}': '{phoneNumber}'");
                    }
                }

                if (!anyInvalid)
                {
                    Console.WriteLine("All distribution list members have valid phone numbers.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
