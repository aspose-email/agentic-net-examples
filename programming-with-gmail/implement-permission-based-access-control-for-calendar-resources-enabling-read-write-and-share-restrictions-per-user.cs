using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize Gmail client (replace placeholders with real credentials)
            Aspose.Email.Clients.Google.IGmailClient gmailClient = GmailClient.GetInstance("YOUR_ACCESS_TOKEN", "user@example.com");
            using (gmailClient)
            {
                // Retrieve the list of calendars
                Aspose.Email.Clients.Google.Calendar[] calendars = gmailClient.ListCalendars();
                if (calendars == null || calendars.Length == 0)
                {
                    Console.WriteLine("No calendars found.");
                    return;
                }

                // Use the first calendar's identifier for the demo
                string calendarId = calendars[0].Id;

                // Create a read‑only access rule
                Aspose.Email.Clients.Google.AccessControlRule readRule = new AccessControlRule(
                    new AclScope(AclScopeType.user, "reader@example.com"),
                    AccessRole.reader);
                gmailClient.CreateAccessRule(calendarId, readRule);

                // Create a write access rule
                Aspose.Email.Clients.Google.AccessControlRule writeRule = new AccessControlRule(
                    new AclScope(AclScopeType.user, "writer@example.com"),
                    AccessRole.writer);
                gmailClient.CreateAccessRule(calendarId, writeRule);

                // Create a share (owner) access rule
                Aspose.Email.Clients.Google.AccessControlRule ownerRule = new AccessControlRule(
                    new AclScope(AclScopeType.user, "owner@example.com"),
                    AccessRole.owner);
                gmailClient.CreateAccessRule(calendarId, ownerRule);

                // List and display all access rules for the calendar
                Aspose.Email.Clients.Google.AccessControlRule[] rules = gmailClient.ListAccessRules(calendarId);
                foreach (Aspose.Email.Clients.Google.AccessControlRule rule in rules)
                {
                    Console.WriteLine($"Scope Type: {rule.Scope.Type}, Email: {rule.Scope.Value}, Role: {rule.Role}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}