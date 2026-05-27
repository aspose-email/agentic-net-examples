using System;
using System.Net;
using System.Net.Sockets;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace AsposeEmailSmtpBindExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // SMTP server configuration (placeholders)
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Skip real network operations when placeholders are present
                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping connection.");
                    return;
                }

                // Resolve a local IPv6 address
                IPAddress localIPv6 = null;
                try
                {
                    IPAddress[] hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                    foreach (IPAddress addr in hostAddresses)
                    {
                        if (addr.AddressFamily == AddressFamily.InterNetworkV6 && !IPAddress.IsLoopback(addr))
                        {
                            localIPv6 = addr;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to resolve local IPv6 address: {ex.Message}");
                }

                if (localIPv6 == null)
                {
                    // Fallback to IPv6 loopback if no other address is found
                    localIPv6 = IPAddress.IPv6Loopback;
                }

                // Create and configure the SMTP client
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    // Bind the client socket to the selected IPv6 address
                    client.BindIPEndPoint += delegate (IPEndPoint remoteEndPoint)
                    {
                        // Use any available local port (0) with the IPv6 address
                        return new IPEndPoint(localIPv6, 0);
                    };

                    // Optional: test connection (commented out to avoid real network call)
                    // client.Noop();

                    Console.WriteLine($"SMTP client bound to local IPv6 address {localIPv6}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
