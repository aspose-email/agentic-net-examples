using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;
using Aspose.Email.PersonalInfo;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the contact file and the encrypted output
            string contactPath = "contact.vcf";
            string encryptedPath = "contact_encrypted.dat";

            // Ensure the directory for the contact file exists
            string contactDirectory = Path.GetDirectoryName(contactPath);
            if (!string.IsNullOrEmpty(contactDirectory) && !Directory.Exists(contactDirectory))
            {
                Directory.CreateDirectory(contactDirectory);
            }

            // Create a sample contact
            Contact contact = new Contact
            {
                GivenName = "John",
                Surname = "Doe"
            };
            contact.EmailAddresses.Add(new EmailAddress("john.doe@example.com"));

            // Save the contact to a VCF file
            try
            {
                contact.Save(contactPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save contact: {ex.Message}");
                return;
            }

            // Verify that the contact file was created
            if (!File.Exists(contactPath))
            {
                Console.Error.WriteLine("Contact file not found.");
                return;
            }

            // Password used for AES encryption
            string password = "P@ssw0rd";

            // Encrypt the contact file using AES and write to encryptedPath
            try
            {
                using (FileStream inputStream = new FileStream(contactPath, FileMode.Open, FileAccess.Read))
                using (FileStream outputStream = new FileStream(encryptedPath, FileMode.Create, FileAccess.Write))
                {
                    // Generate a random salt and write it to the beginning of the output file
                    byte[] salt = new byte[16];
                    using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }
                    outputStream.Write(salt, 0, salt.Length);

                    // Derive a 256‑bit key and a 128‑bit IV from the password and salt
                    using (var pdb = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
                    {
                        byte[] key = pdb.GetBytes(32);
                        byte[] iv = pdb.GetBytes(16);

                        using (Aes aes = Aes.Create())
                        {
                            aes.Key = key;
                            aes.IV = iv;
                            aes.Mode = CipherMode.CBC;
                            aes.Padding = PaddingMode.PKCS7;

                            using (CryptoStream cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                            {
                                inputStream.CopyTo(cryptoStream);
                            }
                        }
                    }
                }

                Console.WriteLine($"Contact file encrypted successfully to '{encryptedPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Encryption failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
