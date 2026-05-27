using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Pop3;

class Program
{
    static void Main()
    {
        try
        {
            // POP3 server parameters
            string host = "pop3.example.com";
            int port = 110;
            string username = "user@example.com";
            string password = "password";
            string logFilePath = "pop3_log.txt";

            // Skip execution when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            // Ensure the directory for the log file exists
            try
            {
                string logDirectory = Path.GetDirectoryName(logFilePath);
                if (!string.IsNullOrEmpty(logDirectory) && !Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare log directory: {ex.Message}");
                return;
            }

            // Create POP3 client and enable logging
            using (Pop3Client client = new Pop3Client(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.EnableLogger = true;
                    client.LogFileName = logFilePath;
                    client.UseDateInLogFileName = false;

                    // Attempt to connect (ValidateCredentials triggers connection)
                    client.ValidateCredentials();

                    // Retrieve server extensions (authentication and encryption support)
                    var supportedAuth = client.SupportedAuthentication;
                    var supportedEncryption = client.SupportedEncryption;

                    // Append extension information to the log file
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(logFilePath, true))
                        {
                            writer.WriteLine($"Connected to {host}:{port} as {username}");
                            writer.WriteLine($"Supported Authentication: {supportedAuth}");
                            writer.WriteLine($"Supported Encryption: {supportedEncryption}");
                            writer.WriteLine($"Log Timestamp: {DateTime.Now}");
                            writer.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write extensions to log file: {ex.Message}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"POP3 client error: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
