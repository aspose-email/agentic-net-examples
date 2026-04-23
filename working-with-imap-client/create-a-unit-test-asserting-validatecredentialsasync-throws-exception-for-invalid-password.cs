using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.Threading.Tasks;
using Aspose.Email.Clients.Imap;

namespace EmailClientValidateTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await RunValidateCredentialsTestAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }

        private static async Task RunValidateCredentialsTestAsync()
        {
            string host = "imap.example.com";
            string username = "testuser";
            string password = "invalidPassword";

            // Skip actual network call when using placeholder host.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder host; test skipped to avoid external call.");
                return;
            }

            using (ImapClient client = new ImapClient(host, username, password))
            {
                bool exceptionThrown = false;
                try
                {
                    // Expect an exception due to invalid credentials.
                    bool result = await client.ValidateCredentialsAsync();
                    Console.Error.WriteLine("Expected exception was not thrown.");
                }
                catch (Exception)
                {
                    exceptionThrown = true;
                }

                if (!exceptionThrown)
                {
                    throw new InvalidOperationException("ValidateCredentialsAsync did not throw an exception for invalid password.");
                }
                else
                {
                    Console.WriteLine("ValidateCredentialsAsync threw an exception as expected.");
                }
            }
        }
    }
}
