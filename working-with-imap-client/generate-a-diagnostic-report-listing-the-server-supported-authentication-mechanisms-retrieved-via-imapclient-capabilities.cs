using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder server and credentials.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used.
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server connection.");
                return;
            }

            // Create the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password))
            {
                try
                {
                    // Retrieve the supported authentication mechanisms.
                    ImapKnownAuthenticationType supportedAuth = client.SupportedAuthentication;

                    // Build a list of individual authentication types.
                    List<string> authList = new List<string>();
                    foreach (ImapKnownAuthenticationType auth in Enum.GetValues(typeof(ImapKnownAuthenticationType)))
                    {
                        if (auth != ImapKnownAuthenticationType.None && (supportedAuth & auth) == auth)
                        {
                            authList.Add(auth.ToString());
                        }
                    }

                    // Output the diagnostic report.
                    Console.WriteLine("Supported IMAP authentication mechanisms:");
                    if (authList.Count > 0)
                    {
                        foreach (string authName in authList)
                        {
                            Console.WriteLine("- " + authName);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No authentication mechanisms reported.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error while retrieving authentication mechanisms: " + ex.Message);
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
