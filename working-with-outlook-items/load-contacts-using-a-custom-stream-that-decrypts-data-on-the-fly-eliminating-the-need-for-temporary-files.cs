using Aspose.Email;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the encrypted contacts file
            string encryptedFilePath = "contacts.enc";

            // Placeholder key and IV for AES decryption (replace with real values)
            byte[] key = new byte[32]; // 256‑bit key (all zeros as placeholder)
            byte[] iv = new byte[16];  // 128‑bit IV (all zeros as placeholder)

            // Ensure the encrypted file exists; if not, create a placeholder encrypted vCard
            if (!File.Exists(encryptedFilePath))
            {
                // Sample vCard content
                string vcard = "BEGIN:VCARD\r\nVERSION:3.0\r\nFN:John Doe\r\nEMAIL:john.doe@example.com\r\nEND:VCARD";
                byte[] plainBytes = Encoding.UTF8.GetBytes(vcard);

                using (Aes aesCreate = Aes.Create())
                {
                    aesCreate.Key = key;
                    aesCreate.IV = iv;

                    using (FileStream fs = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write))
                    using (CryptoStream cs = new CryptoStream(fs, aesCreate.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }
                }

                Console.WriteLine("Placeholder encrypted contacts file created.");
            }

            // Decrypt and load the contact from the encrypted file
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;

                using (FileStream fileStream = new FileStream(encryptedFilePath, FileMode.Open, FileAccess.Read))
                using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    // Load the contact directly from the decrypted stream
                    Contact contact = Contact.Load(cryptoStream);

                    // Output some basic contact information
                    Console.WriteLine("Contact Loaded:");
                    Console.WriteLine("Display Name: " + contact.DisplayName);
                    if (contact.EmailAddresses != null && contact.EmailAddresses.Count > 0)
                    {
                        Console.WriteLine("Email: " + contact.EmailAddresses[0]?.Address);
                    }
                    else
                    {
                        Console.WriteLine("Email: (none)");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
