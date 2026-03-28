using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Validate the supplied credentials
                bool credentialsValid = false;
                if (client is EmailClient emailClient)
                {
                    credentialsValid = emailClient.ValidateCredentials();
                }

                Console.WriteLine(credentialsValid ? "Credentials are valid." : "Invalid credentials.");
                // Further EWS operations can be performed here
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
