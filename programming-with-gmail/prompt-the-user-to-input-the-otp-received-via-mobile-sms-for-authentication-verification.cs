using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailOtpSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – in real scenario replace with actual values.
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string defaultEmail = "user@example.com";

                // Skip external call when using placeholder credentials.
                if (clientId == "clientId" && clientSecret == "clientSecret" && refreshToken == "refreshToken")
                {
                    Console.Error.WriteLine("Placeholder credentials detected – skipping Gmail client operations.");
                    return;
                }

                // Create Gmail client.
                IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
                using (gmailClient as IDisposable)
                {
                    // Prompt user for OTP received via SMS.
                    Console.Write("Enter OTP received via SMS: ");
                    string otp = Console.ReadLine();

                    // In a real scenario the OTP would be used for further verification.
                    Console.WriteLine($"OTP entered: {otp}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
