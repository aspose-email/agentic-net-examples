using System;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using Aspose.Email;

namespace GoogleCredentialsEncryptionSample
{
    // Simple POCO to hold Google user credentials
    public class GoogleCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string RefreshToken { get; set; }
        public string DefaultEmail { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string defaultEmail = "user@example.com";

                // Guard against running with placeholder data
                if (clientId.StartsWith("your-") ||
                    clientSecret.StartsWith("your-") ||
                    refreshToken.StartsWith("your-"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Execution skipped.");
                    return;
                }

                // Assemble credentials object
                GoogleCredentials credentials = new GoogleCredentials
                {
                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    RefreshToken = refreshToken,
                    DefaultEmail = defaultEmail
                };

                // Serialize to JSON
                string json = JsonSerializer.Serialize(credentials);

                // Password used for encryption – in real scenarios obtain securely
                string password = "StrongPassword123!";

                // Derive key and IV from password
                using (Rfc2898DeriveBytes keyDerivation = new Rfc2898DeriveBytes(password, 16, 10000))
                {
                    byte[] key = keyDerivation.GetBytes(32); // AES-256 key
                    byte[] iv = keyDerivation.GetBytes(16);  // AES block size

                    using (Aes aes = Aes.Create())
                    {
                        aes.Key = key;
                        aes.IV = iv;

                        // Encrypt JSON payload
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt, Encoding.UTF8))
                                {
                                    swEncrypt.Write(json);
                                }
                            }

                            byte[] encryptedData = msEncrypt.ToArray();
                            string outputPath = "credentials.enc";

                            // Ensure output directory exists and write encrypted file
                            try
                            {
                                string directory = Path.GetDirectoryName(outputPath);
                                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                                {
                                    Directory.CreateDirectory(directory);
                                }

                                File.WriteAllBytes(outputPath, encryptedData);
                                Console.WriteLine($"Encrypted credentials saved to {outputPath}");
                            }
                            catch (Exception ioEx)
                            {
                                Console.Error.WriteLine($"File write error: {ioEx.Message}");
                                return;
                            }

                            // Demonstrate decryption and deserialization
                            try
                            {
                                byte[] readData = File.ReadAllBytes(outputPath);
                                using (MemoryStream msDecrypt = new MemoryStream(readData))
                                {
                                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aes.CreateDecryptor(), CryptoStreamMode.Read))
                                    {
                                        using (StreamReader srDecrypt = new StreamReader(csDecrypt, Encoding.UTF8))
                                        {
                                            string decryptedJson = srDecrypt.ReadToEnd();
                                            GoogleCredentials loaded = JsonSerializer.Deserialize<GoogleCredentials>(decryptedJson);
                                            Console.WriteLine($"Loaded ClientId: {loaded.ClientId}");
                                        }
                                    }
                                }
                            }
                            catch (Exception decEx)
                            {
                                Console.Error.WriteLine($"Decryption error: {decEx.Message}");
                            }
                        }
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
