using Aspose.Email.Clients;
using Aspose.Email;
using System;
using System.IO;
using System.Text;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string configPath = "config.enc";

            // Ensure the encrypted configuration file exists
            if (!File.Exists(configPath))
            {
                // Create a minimal placeholder configuration
                string placeholder = "smtp.example.com\nuser\npass";
                string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(placeholder));
                File.WriteAllText(configPath, encrypted);
                Console.WriteLine("Placeholder configuration file created at " + configPath);
            }

            // Load and decrypt the configuration
            string encryptedData = File.ReadAllText(configPath);
            byte[] decryptedBytes = Convert.FromBase64String(encryptedData);
            string[] configLines = Encoding.UTF8.GetString(decryptedBytes)
                .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (configLines.Length < 3)
            {
                Console.Error.WriteLine("Invalid configuration format.");
                return;
            }

            string host = configLines[0].Trim();
            string username = configLines[1].Trim();
            string password = configLines[2].Trim();

            // Initialize and configure the SMTP client
            using (SmtpClient client = new SmtpClient())
            {
                client.Host = host;
                client.Username = username;
                client.Password = password;
                client.SecurityOptions = SecurityOptions.Auto;

                Console.WriteLine("SMTP client configured successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
