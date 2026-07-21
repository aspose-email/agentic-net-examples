using Aspose.Email.PersonalInfo;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using System;

class Program
{
    static void Main()
    {
        // Replace with your actual Exchange service URL and credentials
        const string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
        const string username   = "user@example.com";
        const string password   = "password";

        // The unique identifier (URI) of the contact to delete
        const string contactUri = "https://exchange.example.com/EWS/Contacts/ContactId";

        // Guard: skip external calls when placeholder values are detected
        bool hasPlaceholders = serviceUrl.Contains("example.com") ||
                               username.Contains("example.com") ||
                               password == "password";

        if (hasPlaceholders)
        {
            Console.WriteLine("Placeholder credentials detected. Skipping contact deletion.");
            return;
        }

        try
        {
            // Create the Exchange Web Services client (IDisposable)
            using (IEWSClient exchangeClient = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Delete the contact using the generic DeleteItem method
                exchangeClient.DeleteItem(contactUri, new DeletionOptions(DeletionType.Default));
            }

            Console.WriteLine("Contact deleted successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error deleting contact: {ex.Message}");
        }
    }
}
