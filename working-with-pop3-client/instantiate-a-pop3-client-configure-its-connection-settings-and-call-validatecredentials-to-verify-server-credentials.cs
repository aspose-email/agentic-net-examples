using System;
using Aspose.Email.Clients.Pop3;

namespace AsposeEmailPop3Validate
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Connection settings
                string host = "pop.example.com";
                int port = 110;
                string username = "user@example.com";
                string password = "password";

                // Initialize POP3 client
                using (Pop3Client client = new Pop3Client(host, port, username, password))
                {
                    try
                    {
                        // Validate credentials
                        bool isValid = client.ValidateCredentials();
                        Console.WriteLine(isValid ? "Credentials are valid." : "Credentials are invalid.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Credential validation failed: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
