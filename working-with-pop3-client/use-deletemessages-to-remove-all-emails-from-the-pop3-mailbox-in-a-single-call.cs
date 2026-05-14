using Aspose.Email;
using System;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // POP3 server connection details (replace with real values)
            string host = "pop3.example.com";
            string username = "username";
            string password = "password";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping POP3 operations.");
                return;
            }

            // Create and use the POP3 client
            using (Pop3Client client = new Pop3Client(host, username, password))
            {
                try
                {
                    // Mark all messages for deletion
                    client.DeleteMessages();
                    Console.WriteLine("All messages have been marked for deletion.");

                    // Commit the deletions (moves the session to UPDATE state)
                    Console.WriteLine("Deletions have been committed.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 operation error: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
