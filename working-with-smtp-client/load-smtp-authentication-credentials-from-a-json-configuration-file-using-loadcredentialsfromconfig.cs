using Aspose.Email.Clients;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using System;
using System.IO;
using System.Text.Json;

class Program
{
    static void Main()
    {
        try
        {
            // Load SMTP credentials from configuration file
            SmtpConfig config = LoadCredentialsFromConfig("smtp_config.json");
            if (config == null)
            {
                // Configuration could not be loaded; exit gracefully
                return;
            }

            // Guard against placeholder credentials to avoid real network calls in CI
            if (string.IsNullOrWhiteSpace(config.Host) ||
                config.Host.Contains("example.com", StringComparison.OrdinalIgnoreCase) ||
                string.IsNullOrWhiteSpace(config.Username) ||
                string.IsNullOrWhiteSpace(config.Password))
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping connection.");
                return;
            }

            // Create and use the SmtpClient inside a using block
            using (SmtpClient client = new SmtpClient(config.Host, config.Port, config.Username, config.Password, config.Security))
            {
                try
                {
                    // Validate the credentials
                    bool valid = client.ValidateCredentials();
                    Console.WriteLine(valid ? "SMTP credentials are valid." : "SMTP credentials are invalid.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during SMTP validation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Loads SMTP configuration from a JSON file.
    // If the file does not exist, creates a minimal placeholder and returns null.
    private static SmtpConfig LoadCredentialsFromConfig(string path)
    {
        try
        {
            if (!File.Exists(path))
            {
                // Ensure the output directory exists
                string fullPath = Path.GetFullPath(path);
                string directory = Path.GetDirectoryName(fullPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Create a minimal placeholder configuration
                var placeholder = new SmtpConfig
                {
                    Host = "smtp.example.com",
                    Port = 25,
                    Username = "user@example.com",
                    Password = "password",
                    Security = SecurityOptions.Auto
                };
                string json = JsonSerializer.Serialize(placeholder, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);
                Console.Error.WriteLine($"Configuration file not found. Placeholder created at '{path}'.");
                return null;
            }

            string content = File.ReadAllText(path);
            SmtpConfig config = JsonSerializer.Deserialize<SmtpConfig>(content);
            return config;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to load configuration: {ex.Message}");
            return null;
        }
    }

    // Simple POCO to hold SMTP settings
    private class SmtpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public SecurityOptions Security { get; set; }
    }
}
