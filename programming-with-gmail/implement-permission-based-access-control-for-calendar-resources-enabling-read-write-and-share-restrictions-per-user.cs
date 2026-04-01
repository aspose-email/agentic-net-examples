using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder OAuth credentials
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Skip real network calls when placeholders are used
            if (clientId == "clientId" || clientSecret == "clientSecret" || refreshToken == "refreshToken")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client instance
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            try
            {
                // Calendar identifier (use "primary" for the default calendar)
                string calendarId = "primary";

                // Define a user to grant read access
                string userEmail = "alice@example.com";

                // Construct ACL scope using the enum (not a string)
                AclScope scope = new AclScope(AclScopeType.user, userEmail);

                // Create an access control rule with read permission
                AccessControlRule rule = new AccessControlRule(scope, AccessRole.reader);

                // Add the rule to the calendar
                AccessControlRule createdRule = gmailClient.CreateAccessRule(calendarId, rule);
                Console.WriteLine($"Created rule for {createdRule.Scope.Value} with role {createdRule.Role}");

                // List all ACL rules for the calendar
                AccessControlRule[] existingRules = gmailClient.ListAccessRules(calendarId);
                foreach (AccessControlRule existingRule in existingRules)
                {
                    Console.WriteLine($"User: {existingRule.Scope.Value}, Role: {existingRule.Role}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
            }
            finally
            {
                if (gmailClient != null)
                {
                    gmailClient.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
