using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;

namespace EmailVerificationSample
{
    class Program
    {
        static void Main()
        {
            // Author note: Simple IMAP server connectivity verification using Aspose.Email.
            string imapHost = "imap.example.com";
            string imapUsername = "user@example.com";
            string imapPassword = "password";

            // Guard against placeholder credentials – skip network call if they are not real.
            if (imapHost.Contains("example") || imapUsername.Contains("example"))
            {
                Console.WriteLine("Skipping verification due to placeholder credentials.");
                return;
            }

            try
            {
                // Create the IMAP client with automatic security selection.
                using (ImapClient client = new ImapClient(imapHost, imapUsername, imapPassword, SecurityOptions.Auto))
                {
                    // Attempt to select the INBOX folder; this forces a connection and validates credentials.
                    client.SelectFolder("INBOX");
                    Console.WriteLine("IMAP server verification succeeded.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"IMAP server verification failed: {ex.Message}");
            }
        }
    }
}
