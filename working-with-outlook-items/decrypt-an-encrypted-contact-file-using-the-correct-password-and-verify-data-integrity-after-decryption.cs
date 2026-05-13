using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string encryptedFilePath = "encrypted_contact.msg";

            if (!File.Exists(encryptedFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(encryptedFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {encryptedFilePath}");
                return;
            }

            using (MapiMessage encryptedMessage = MapiMessage.Load(encryptedFilePath))
            {
                if (!encryptedMessage.IsEncrypted)
                {
                    Console.WriteLine("The message is not encrypted.");
                    return;
                }

                using (MapiMessage decryptedMessage = encryptedMessage.Decrypt())
                {
                    // Verify integrity by computing SHA256 hash of the body content
                    using (SHA256 sha256 = SHA256.Create())
                    {
                        string bodyText = decryptedMessage.Body ?? string.Empty;
                        byte[] bodyBytes = Encoding.UTF8.GetBytes(bodyText);
                        byte[] hashBytes = sha256.ComputeHash(bodyBytes);
                        string hashString = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
                        Console.WriteLine($"SHA256 hash of decrypted body: {hashString}");
                    }

                    string outputPath = "decrypted_contact.msg";
                    decryptedMessage.Save(outputPath);
                    Console.WriteLine($"Decrypted message saved to: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
