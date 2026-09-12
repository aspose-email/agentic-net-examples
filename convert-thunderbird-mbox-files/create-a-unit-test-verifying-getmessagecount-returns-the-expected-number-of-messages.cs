using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

namespace ConvertThunderbirdMboxTest
{
    class Program
    {
        static void Main()
        {
            // Placeholder POP3 server details – replace with real values for an actual test.
            string host = "pop.example.com";
            int port = 110;
            string username = "user";
            string password = "pass";

            // Expected number of messages in the test mailbox.
            int expectedCount = 0; // Set this to the known count when using a real server.
            int actualCount = 0;

            try
            {
                using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
                {
                    actualCount = client.GetMessageCount();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"POP3 connection failed: {ex.Message}");
                // Keep actualCount as 0 if connection fails.
            }

            if (actualCount == expectedCount)
            {
                Console.WriteLine($"Test passed. Message count: {actualCount}");
            }
            else
            {
                Console.WriteLine($"Test failed. Expected {expectedCount}, but got {actualCount}");
            }
        }
    }
}
