using System;
using System.IO;
using System.Text;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailSession
{
    // Simple model representing stored Google authentication data.
    public class GoogleUser
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string DefaultEmail { get; set; }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                const string encryptedFilePath = "google_user_encrypted.json";

                // Ensure the encrypted file exists; create a minimal placeholder if missing.
                if (!File.Exists(encryptedFilePath))
                {
                    try
                    {
                        GoogleUser placeholder = new GoogleUser
                        {
                            AccessToken = "PLACEHOLDER_ACCESS_TOKEN",
                            RefreshToken = "PLACEHOLDER_REFRESH_TOKEN",
                            ClientId = "PLACEHOLDER_CLIENT_ID",
                            ClientSecret = "PLACEHOLDER_CLIENT_SECRET",
                            DefaultEmail = "user@example.com"
                        };
                        string json = JsonSerializer.Serialize(placeholder);
                        // Simple "encryption": Base64 encode the JSON.
                        string encrypted = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                        File.WriteAllText(encryptedFilePath, encrypted);
                        Console.Error.WriteLine($"Placeholder encrypted file created at '{encryptedFilePath}'. Populate it with real data before running again.");
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                        return;
                    }
                }

                // Read and decrypt the stored JSON.
                GoogleUser googleUser;
                try
                {
                    string encryptedContent = File.ReadAllText(encryptedFilePath);
                    // Simple "decryption": Base64 decode.
                    byte[] decodedBytes = Convert.FromBase64String(encryptedContent);
                    string json = Encoding.UTF8.GetString(decodedBytes);
                    googleUser = JsonSerializer.Deserialize<GoogleUser>(json);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error reading or decrypting the file: {ex.Message}");
                    return;
                }

                // Validate that we have real credentials; skip if placeholders are present.
                if (string.IsNullOrWhiteSpace(googleUser?.AccessToken) ||
                    googleUser.AccessToken.StartsWith("PLACEHOLDER", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("Google authentication data contains placeholder values. Provide real credentials and retry.");
                    return;
                }

                // Create the Gmail client using the stored access token.
                IGmailClient gmailClient = null;
                try
                {
                    gmailClient = GmailClient.GetInstance(googleUser.AccessToken, googleUser.DefaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                // Use the client within a using block to ensure proper disposal.
                using (gmailClient)
                {
                    try
                    {
                        // Example operation: list messages to verify the session.
                        System.Collections.Generic.List<GmailMessageInfo> messages = gmailClient.ListMessages();
                        Console.WriteLine($"Successfully retrieved {messages.Count} message(s) for user '{googleUser.DefaultEmail}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error during Gmail operation: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
