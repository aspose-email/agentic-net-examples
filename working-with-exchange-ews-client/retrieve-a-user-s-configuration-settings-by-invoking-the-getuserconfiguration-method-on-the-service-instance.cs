using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder service URL and credentials.
            string serviceUrl = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing with placeholder values.
            if (serviceUrl.Contains("example"))
            {
                Console.Error.WriteLine("Placeholder service URL detected. Skipping execution.");
                return;
            }

            // Create the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                try
                {
                    // Resolve the enum value at runtime to avoid compile‑time dependency on a specific member.
                    UserConfigurationName configName = (UserConfigurationName)Enum.Parse(
                        typeof(UserConfigurationName), "InboxRules");

                    // Retrieve the user configuration.
                    UserConfiguration config = client.GetUserConfiguration(configName);

                    // Simple confirmation output.
                    Console.WriteLine("User configuration retrieved successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving configuration: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
