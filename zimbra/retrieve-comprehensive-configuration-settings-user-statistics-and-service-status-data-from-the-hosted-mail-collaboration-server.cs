using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.PersonalInfo;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        string email = "user@example.com";

        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Connection parameters – replace with real values
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and use the EWS client
            using (IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Server version information
                string versionInfo = ewsClient.GetVersionInfo();
                Console.WriteLine("Exchange Server Version: " + versionInfo);

                // Mailbox information
                ExchangeMailboxInfo mailboxInfo = ewsClient.GetMailboxInfo();
                Console.WriteLine("Mailbox Info: " + mailboxInfo);

                // Mailbox size (bytes)
                long mailboxSize = ewsClient.GetMailboxSize();
                Console.WriteLine("Mailbox Size (bytes): " + mailboxSize);

                // List of mailboxes (as contacts)
                Contact[] mailboxes = ewsClient.GetMailboxes();
                Console.WriteLine("Number of mailboxes retrieved: " + mailboxes.Length);
                foreach (Contact contact in mailboxes)
                {
                    Console.WriteLine("Mailbox: " + contact);
                }

                // Retrieve configuration settings via Autodiscover
                AutodiscoverService autodiscover = new AutodiscoverService();
                try
                {
                    GetUserSettingsResponse settingsResponse = autodiscover.GetUserSettings(
                        username,
                        UserSettingName.InternalEwsUrl,
                        UserSettingName.ExternalEwsUrl,
                        UserSettingName.InternalMailboxServer,
                        UserSettingName.ExternalMailboxServer);

                    Console.WriteLine("Autodiscover Settings:");
                    UserSettingName[] requestedSettings = new UserSettingName[]
                    {
                        UserSettingName.InternalEwsUrl,
                        UserSettingName.ExternalEwsUrl,
                        UserSettingName.InternalMailboxServer,
                        UserSettingName.ExternalMailboxServer
                    };

                    foreach (UserSettingName settingName in requestedSettings)
                    {
                        object value;
                        if (settingsResponse.TryGetSettingValue<object>(settingName, out value))
                        {
                            Console.WriteLine($"{settingName}: {value}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Autodiscover error: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}