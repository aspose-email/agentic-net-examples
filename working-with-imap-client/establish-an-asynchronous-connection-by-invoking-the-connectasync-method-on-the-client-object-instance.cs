using Aspose.Email.Clients;
using System;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip actual connection in sample runs
            string host = "imap.example.com";
            int port = 993;
            string username = "username";
            string password = "password";

            if (host == "imap.example.com")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            // Establish an asynchronous connection and validate credentials
            using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.Auto))
            {
                bool isValid = await client.ValidateCredentialsAsync();
                Console.WriteLine($"Connection validated: {isValid}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
