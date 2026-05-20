using Aspose.Email.Clients;
using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Imap;

namespace AsposeEmailExtensionsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string host = "imap.example.com";
                int port = 993;
                string username = "user@example.com";
                string password = "password";

                // Guard against placeholder credentials to avoid real network calls during CI
                if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping server connection.");
                    return;
                }

                // Initialize and connect the IMAP client (constructor performs connection and authentication)
                using (ImapClient client = new ImapClient(host, port, username, password, SecurityOptions.SSLImplicit))
                {
                    // Retrieve server extensions and store them in a dictionary
                    Dictionary<string, bool> extensions = new Dictionary<string, bool>
                    {
                        ["AnnotateSupported"] = client.AnnotateSupported,
                        ["ChildrenSupported"] = client.ChildrenSupported,
                        ["CompressSupported"] = client.CompressSupported,
                        ["CondstoreSupported"] = client.CondstoreSupported,
                        ["IdSupported"] = client.IdSupported,
                        ["MoveSupported"] = client.MoveSupported,
                        ["NamespaceSupported"] = client.NamespaceSupported,
                        ["QresyncSupported"] = client.QresyncSupported,
                        ["QuotaSupported"] = client.QuotaSupported,
                        ["SaslIrSupported"] = client.SaslIrSupported,
                        ["SortSupported"] = client.SortSupported,
                        ["SpecialUseSupported"] = client.SpecialUseSupported,
                        ["ThreadSupported"] = client.ThreadSupported,
                        ["UidPlusSupported"] = client.UidPlusSupported,
                        ["UnselectSupported"] = client.UnselectSupported,
                        ["EnableSupported"] = client.EnableSupported
                    };

                    // Example usage: check if a specific extension is supported
                    string checkKey = "MoveSupported";
                    if (extensions.TryGetValue(checkKey, out bool isSupported) && isSupported)
                    {
                        Console.WriteLine($"{checkKey} is supported by the server.");
                    }
                    else
                    {
                        Console.WriteLine($"{checkKey} is not supported by the server.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
