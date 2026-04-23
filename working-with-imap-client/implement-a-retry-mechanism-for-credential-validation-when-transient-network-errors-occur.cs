using System;
using System.Threading;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values.
            string host = "imap.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder values are detected to avoid real network calls.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create the IMAP client.
            using (ImapClient client = new ImapClient(host, username, password, SecurityOptions.Auto))
            {
                const int maxRetryAttempts = 3;
                const int delayBetweenAttemptsMs = 2000;
                bool credentialsValid = false;

                for (int attempt = 1; attempt <= maxRetryAttempts; attempt++)
                {
                    try
                    {
                        // Attempt to validate credentials.
                        credentialsValid = client.ValidateCredentials();
                        if (credentialsValid)
                        {
                            Console.WriteLine("Credentials are valid.");
                            break;
                        }
                        else
                        {
                            Console.Error.WriteLine("Credentials are invalid.");
                            break;
                        }
                    }
                    catch (ImapException imapEx)
                    {
                        // Transient network error – retry if attempts remain.
                        Console.Error.WriteLine($"Attempt {attempt} failed with IMAP error: {imapEx.Message}");
                        if (attempt == maxRetryAttempts)
                        {
                            Console.Error.WriteLine("Maximum retry attempts reached. Validation failed.");
                            break;
                        }
                        Thread.Sleep(delayBetweenAttemptsMs);
                    }
                    catch (Exception ex)
                    {
                        // Non‑transient error – abort retries.
                        Console.Error.WriteLine($"Unexpected error during validation: {ex.Message}");
                        break;
                    }
                }

                // Additional logic can be placed here, using the client if needed.
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
