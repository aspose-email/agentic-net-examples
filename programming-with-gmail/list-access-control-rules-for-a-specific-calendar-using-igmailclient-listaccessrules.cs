using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string accessToken = "YOUR_ACCESS_TOKEN";
            string defaultEmail = "user@example.com";
            string calendarId = "primary";

            // Guard against placeholder credentials.
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client call.");
                return;
            }

            // Create Gmail client safely.
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(accessToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Use the client within a using block to ensure disposal.
            using (gmailClient)
            {
                AccessControlRule[] rules = null;
                try
                {
                    rules = gmailClient.ListAccessRules(calendarId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error retrieving access rules: {ex.Message}");
                    return;
                }

                if (rules == null || rules.Length == 0)
                {
                    Console.WriteLine("No access control rules found for the specified calendar.");
                    return;
                }

                Console.WriteLine($"Access control rules for calendar '{calendarId}':");
                foreach (AccessControlRule rule in rules)
                {
                    string scopeInfo = rule.Scope != null ? rule.Scope.ToString() : "null";
                    Console.WriteLine($"Id: {rule.Id}, Role: {rule.Role}, Scope: {scopeInfo}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
