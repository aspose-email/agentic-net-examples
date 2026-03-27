using System;
using Aspose.Email.Clients.Google;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder OAuth 2.0 access token and user email.
                string accessToken = "accessToken";
                string defaultEmail = "user@example.com";

                // Create Gmail client.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Placeholder contact URI to be deleted.
                    string contactUri = "contactUri";

                    // Delete the specified contact.
                    gmailClient.DeleteContact(contactUri);
                    Console.WriteLine("Contact deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}