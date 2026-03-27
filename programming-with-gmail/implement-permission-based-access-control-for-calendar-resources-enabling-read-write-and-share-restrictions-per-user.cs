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
            // Initialize Gmail client (dummy credentials)
            using (IGmailClient gmailClient = GmailClient.GetInstance(
                "clientId",
                "clientSecret",
                "refreshToken",
                "user@example.com"))
            {
                // Create a new calendar
                Calendar calendar = new Calendar("Sample Calendar");
                string calendarId = gmailClient.CreateCalendar(calendar);
                Console.WriteLine($"Created calendar with Id: {calendarId}");

                // Define a scope for a specific user
                AclScope scope = new AclScope(AclScopeType.user, "alice@example.com");

                // Create a read‑only access rule for the user
                AccessControlRule readRule = new AccessControlRule(scope, AccessRole.reader);
                AccessControlRule createdRule = gmailClient.CreateAccessRule(calendarId, readRule);
                Console.WriteLine($"Access rule created: Role={createdRule.Role}, Id={createdRule.Id}");

                // Update the rule to give write permission
                AccessControlRule writeRule = new AccessControlRule(scope, AccessRole.writer);
                AccessControlRule updatedRule = gmailClient.UpdateAccessRule(calendarId, writeRule);
                Console.WriteLine($"Access rule updated: Role={updatedRule.Role}, Id={updatedRule.Id}");

                // Delete the access rule
                gmailClient.DeleteAccessRule(calendarId, createdRule.Id);
                Console.WriteLine("Access rule deleted.");

                // Clean up the calendar
                gmailClient.DeleteCalendar(calendarId);
                Console.WriteLine("Calendar deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
