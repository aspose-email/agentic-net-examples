using System;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailVerificationSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Capture user inputs
                Console.Write("Enter your country: ");
                string country = Console.ReadLine();

                Console.Write("Enter your mobile number: ");
                string mobileNumber = Console.ReadLine();

                // Create a MAPI contact and set the required properties
                using (MapiContact contact = new MapiContact())
                {
                    contact.SetProperty(KnownPropertyList.Country, country);
                    contact.SetProperty(KnownPropertyList.MobileTelephoneNumber, mobileNumber);
                }

                // Placeholder EWS connection details
                string ewsUrl = "https://ews.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";
                string domain = "";

                // Guard against executing real network calls with placeholder data
                if (ewsUrl.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder EWS credentials detected. Skipping external call.");
                    return;
                }

                // Create EWS client and invoke PlayOnPhone
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, username, password, domain))
                    {
                        // Placeholder message identifier
                        string messageId = "sampleMessageId";

                        client.PlayOnPhone(messageId, mobileNumber);
                        Console.WriteLine("Verification code request sent successfully.");
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
}
