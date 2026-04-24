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
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "user@example.com";

            // Guard against running with placeholder data.
            if (clientId.StartsWith("your-") ||
                clientSecret.StartsWith("your-") ||
                refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Define the scope of the access rule (grant read permission to a user).
                AclScope scope = new AclScope(AclScopeType.user, "reader@example.com");

                // Create the access control rule with the Reader role.
                AccessControlRule rule = new AccessControlRule(scope, AccessRole.reader);

                // Calendar identifier – using "primary" as the default calendar.
                string calendarId = "primary";

                try
                {
                    // Update the access rule on the specified calendar.
                    AccessControlRule updatedRule = gmailClient.UpdateAccessRule(calendarId, rule);
                    Console.WriteLine($"Access rule updated. Id: {updatedRule.Id}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to update access rule: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
